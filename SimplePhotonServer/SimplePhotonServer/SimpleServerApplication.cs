using Photon.SocketServer;
using System;
using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;

namespace SimplePhotonServer
{
    public class SimpleServerApplication : ApplicationBase
    {
        private readonly ILogger Log = LogManager.GetCurrentClassLogger();

        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            Log.Info("Create peer");
            return new UnityClient(initRequest.Protocol,initRequest.PhotonPeer);
        }

        protected override void Setup()
        {
            var file = new FileInfo(Path.Combine(BinaryPath,"log4net.config"));
            if (file.Exists) {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);
            }
            Log.Info("Server is ready");
        }

        protected override void TearDown()
        {
            Log.Info("Server Tear Down");
        }
    }
}
