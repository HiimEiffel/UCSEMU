/*
 * Program : Ultrapowa Clash Server
 * Description : A C# Writted 'Clash of Clans' Server Emulator !
 *
 * Authors:  Jean-Baptiste Martin <Ultrapowa at Ultrapowa.com>,
 *           And the Official Ultrapowa Developement Team
 *
 * Copyright (c) 2016  UltraPowa
 * All Rights Reserved.
 */

using System.IO;
using UCS.Core;
using UCS.Core.Network;
using UCS.Helpers;
using UCS.Logic;
using UCS.PacketProcessing.Messages.Server;

namespace UCS.PacketProcessing.Messages.Client
{
    internal class VisitHomeMessage : Message
    {
        #region Public Constructors

        public VisitHomeMessage(PacketProcessing.Client client, CoCSharpPacketReader br)
            : base(client, br)
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public long AvatarId { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override void Decode()
        {
            using (var br = new BinaryReader(new MemoryStream(GetData())))
            {
                AvatarId = br.ReadInt64WithEndian();
            }
        }

        public override void Process(Level level)
        {
            var targetLevel = ResourcesManager.GetPlayer(AvatarId);
            targetLevel.Tick();
            var clan = ObjectManager.GetAlliance(level.GetPlayerAvatar().GetAllianceId());
            PacketManager.ProcessOutgoingPacket(new VisitedHomeDataMessage(Client, targetLevel, level));
            if (clan != null)
                PacketManager.ProcessOutgoingPacket(new AllianceStreamMessage(Client, clan));
        }

        #endregion Public Methods
    }
}