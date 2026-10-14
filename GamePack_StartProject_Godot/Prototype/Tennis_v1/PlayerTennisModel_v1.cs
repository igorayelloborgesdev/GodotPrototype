using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackStartProjectGodot.Prototype.Tennis_v1
{
    public class PlayerTennisModel_v1
    {
        public Vector3 direction = Vector3.Zero;
        public ServeEnum serveEnum = ServeEnum.Stand;
        public enum ServeEnum
        {
            Stand = 0,
            ThrowBall = 1,
            HitBall = 2,
            InGame = 3
        }
        public ServeDirectionEnum serveDirectionEnum = ServeDirectionEnum.Center;
        public enum ServeDirectionEnum
        {
            Center = 0,
            Left = 1,
            Right = 2
        }
        public BackForeHandDirectionEnum backForeHandDirectionEnum = BackForeHandDirectionEnum.Center;
        public enum BackForeHandDirectionEnum
        {
            CenterDown = 0,
            Center = 1,
            CenterUp = 2,
            LeftDown = 3,
            Left = 4,
            LeftUp = 5,
            RightDown = 6,
            Right = 7,
            RightUp = 8
        }

        public ServeEnumWait serveEnumWait = ServeEnumWait.Init;
        public enum ServeEnumWait
        {
            Init = 0,
            Act = 1
        }
    }
}
