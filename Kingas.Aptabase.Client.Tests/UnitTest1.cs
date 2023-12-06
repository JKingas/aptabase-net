using Aptabase;

namespace Kingas.Aptabase.Client.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var client = new AptabaseClient("T-DEV-Test", null, null);
            client.TrackEvent("test");
            Thread.Sleep(5000);
        }
    }
}