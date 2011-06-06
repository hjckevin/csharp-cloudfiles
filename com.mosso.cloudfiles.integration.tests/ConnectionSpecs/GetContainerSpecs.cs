using System.Collections.Generic;
using com.mosso.cloudfiles.domain;
using NUnit.Framework;

namespace com.mosso.cloudfiles.integration.tests.ConnectionSpecs
{
    [TestFixture]
    public class when_getting_container_lists_without_parameters
    {
        private IConnection _connection;

        [SetUp]
        public void Setup()
        {
            _connection = new Connection(new UserCredentials(Credentials.USERNAME, Credentials.API_KEY));
        }

        [Test]
        public void should_get_them_all()
        {
            var containers = _connection.GetContainers();

            Assert.That(containers.Count, Is.GreaterThan(0));
        }
    }

    [TestFixture]
    public class when_getting_container_lists_with_parameters
    {
        private IConnection _connection;
        private Dictionary<GetListParameters, string> _parameters;

        [SetUp]
        public void Setup()
        {
            _connection = new Connection(new UserCredentials(Credentials.USERNAME, Credentials.API_KEY));
        }

        [Test]
        public void should_get_the_last_container_when_marker_set_to_next_to_last()
        {
            var containers = _connection.GetContainers();
            var lastContainerName = containers.FindLast(x => !string.IsNullOrEmpty(x));
            _parameters = new Dictionary<GetListParameters, string> { { GetListParameters.Marker, containers[containers.Count - 2] } };
            containers = _connection.GetContainers(_parameters);

            Assert.That(containers.Count, Is.EqualTo(1));
            Assert.That(containers[0], Is.EqualTo(lastContainerName));
        }

        [Test]
        public void should_get_no_containers_when_last_container_is_passed_in_marker_parameter()
        {
            var containers = _connection.GetContainers();
            _parameters = new Dictionary<GetListParameters, string> { { GetListParameters.Marker, containers[containers.Count - 1] } };
            containers = _connection.GetContainers(_parameters);

            Assert.That(containers.Count, Is.EqualTo(0));
        }

        [Test]
        public void should_get_the_all_containers_using_marker_parameter()
        {
            var allContainers = new List<string>();
            _parameters = null;
            var containers = GetContainers(allContainers, _parameters);

            Assert.That(containers.Count, Is.GreaterThan(0));
        }

        private List<string> GetContainers(List<string> containers, Dictionary<GetListParameters, string> parameters)
        {
            var list = _connection.GetContainers(parameters);
            if (list.Count == 0) return containers;
            containers.AddRange(list);
            var lastContainerName = containers.FindLast(x => !string.IsNullOrEmpty(x));
            parameters = new Dictionary<GetListParameters, string> { { GetListParameters.Marker, lastContainerName } };
            return GetContainers(containers, parameters);
        }

        [Test]
        public void should_only_get_2_containers_when_limit_set_to_2()
        {
            _parameters = new Dictionary<GetListParameters, string> { { GetListParameters.Limit, "2" } };
            var containers = _connection.GetContainers(_parameters);

            Assert.That(containers.Count, Is.EqualTo(2));
        }
    }
}