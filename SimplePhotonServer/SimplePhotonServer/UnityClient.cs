using Photon.SocketServer;
using System;
using System.Collections.Generic;
using PhotonHostRuntimeInterfaces;
using ExitGames.Logging;
using System.Timers;

namespace SimplePhotonServer
{
    class UnityClient : PeerBase
    {
        enum EvCode : byte
        {
            UpdateStats
        }

        enum OpCode : byte
        {
            SetCharacterHealth,
            RequestHit
        }

        enum OpParam : byte
        {
            CharacterName,
            CharacterHealth,
            CanHit
        }

        private readonly ILogger Log = LogManager.GetCurrentClassLogger();
        
        private Dictionary<string, byte> charactersHealths;

        private int updateStatsTimeMs = 60000;
        Timer updateStatsTimer;

        public UnityClient(IRpcProtocol protocol, IPhotonPeer unmanagedPeer) : base(protocol, unmanagedPeer)
        {
            Log.Info("Player connection ip: "+unmanagedPeer.GetRemoteIP());
            charactersHealths = new Dictionary<string, byte>();
            setUpdateStatsTimer();
        }

        public void setUpdateStatsTimer() {
            updateStatsTimer = new Timer(updateStatsTimeMs);
            updateStatsTimer.Elapsed += SendUpdateStatsEvent;
            updateStatsTimer.AutoReset = true;
            updateStatsTimer.Enabled = true;
        }


        private void SendUpdateStatsEvent(Object source, ElapsedEventArgs e)
        {
            Log.Info(String.Format("send update stats event at {0:HH:mm:ss.fff}",
                              e.SignalTime));
            EventData eventdata = new EventData((byte)EvCode.UpdateStats);
            SendEvent(eventdata, new SendParameters());
        }

        protected override void OnDisconnect(DisconnectReason reasonCode, string reasonDetail)
        {
            Log.Info("Player disconnected "+reasonDetail);
            updateStatsTimer.Stop();
            updateStatsTimer.Dispose();
        }

        protected override void OnOperationRequest(OperationRequest operationRequest, SendParameters sendParameters)
        {
            Log.Debug("OnOperationRequest "+operationRequest.OperationCode);
            switch (operationRequest.OperationCode) {
                case (byte)OpCode.SetCharacterHealth:
                    string characterName = (string)operationRequest.Parameters[(byte)OpParam.CharacterName];
                    byte characterHealth = (byte)operationRequest.Parameters[(byte)OpParam.CharacterHealth];
                    if (charactersHealths.ContainsKey(characterName))
                    {
                        charactersHealths[characterName] = characterHealth;
                    }
                    else
                        charactersHealths.Add(characterName,characterHealth);
                    break;
                case (byte)OpCode.RequestHit:
                    RequestHit((string)operationRequest.Parameters[(byte)OpParam.CharacterName],sendParameters);
                    break;
            }
        }

        void RequestHit(string characterName, SendParameters sendParameters) {
            if (!charactersHealths.ContainsKey(characterName))
                return;
            OperationResponse response = new OperationResponse((byte)OpCode.RequestHit);
            byte currentHealth = charactersHealths[characterName];
            Dictionary<byte, object> responseParams = new Dictionary<byte, object>();
            responseParams.Add((byte)OpParam.CanHit,currentHealth!=0);
            if (currentHealth > 0) {
                currentHealth--;
                charactersHealths[characterName] = currentHealth;
                responseParams.Add((byte)OpParam.CharacterName,characterName);
                responseParams.Add((byte)OpParam.CharacterHealth,currentHealth);
            }
            response.Parameters = responseParams;
            SendOperationResponse(response,sendParameters);
        }
    }
}
