using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using log4net;

namespace NMAP
{
    public class TPLScanner : IPScanner
    {
        protected virtual ILog log => LogManager.GetLogger(typeof(SequentialScanner));

        public virtual Task Scan(IPAddress[] ipAddrs, int[] ports)
        {
        	return Task.Run(async () =>
            {
	            return Task.WhenAll(ipAddrs.Select(addres => CheckIpAddress(addres, ports)));
            });
        }

        private async Task CheckIpAddress(IPAddress address, int[] ports)
        {

	        if(await PingAddr(address) != IPStatus.Success)
		        return;

	        await Task.WhenAll(ports.Select(port => CheckPort(address, port)));
        }

        protected async Task<IPStatus> PingAddr(IPAddress ipAddr, int timeout = 3000)
        {
        	log.Info($"Pinging {ipAddr}");
        	using(var ping = new Ping())
        	{
        		var status = (await ping.SendPingAsync(ipAddr, timeout)).Status;
                log.Info($"Pinged {ipAddr}: {status}");
        		return status;
        	}
        }

        protected async Task<PortStatus> CheckPort(IPAddress ipAddr, int port, int timeout = 3000)
        {
        	using(var tcpClient = new TcpClient())
        	{
        		log.Info($"Checking {ipAddr}:{port}");

        		var connectTask = await tcpClient.ConnectWithTimeoutAsync(ipAddr, port, timeout);
                PortStatus portStatus;
        		switch(connectTask.Status)
        		{
        			case TaskStatus.RanToCompletion:
        				portStatus = PortStatus.OPEN;
        				break;
        			case TaskStatus.Faulted:
        				portStatus = PortStatus.CLOSED;
        				break;
        			default:
        				portStatus = PortStatus.FILTERED;
        				break;
        		}
        		log.Info($"Checked {ipAddr}:{port} - {portStatus}");
                return portStatus;
            }
        }
    }
}