﻿using RaceElement.Data.SharedMemory;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


// Used parts of: https://github.com/gro-ove/actools/tree/master/AcManager.Tools/SharedMemory
namespace RaceElement.Data.Games.AssettoCorsa.SharedMemory
{
    internal static class AcSharedMemory
    {
        private static readonly string physicsMap = "Local\\acpmf_physics";
        private static readonly string graphicsMap = "Local\\acpmf_graphics";
        private static readonly string staticMap = "Local\\acpmf_static";

        public static PageFileStatic StaticPage { get; private set; }
        public static PageFilePhysics PhysicsPage { get; private set; }
        public static PageFileGraphics GraphicsPage { get; private set; }

        public enum AcStatus : int
        {
            AC_OFF,
            AC_REPLAY,
            AC_LIVE,
            AC_PAUSE,
        }

        public enum AcSessionType : int
        {
            AC_UNKNOWN = -1,
            AC_PRACTICE = 0,
            AC_QUALIFY = 1,
            AC_RACE = 2,
            AC_HOTLAP = 3,
            AC_TIME_ATTACK = 4,
            AC_DRIFT = 5,
            AC_DRAG = 6,
            AC_HOTSTINT = 7,
            AC_HOTLAPSUPERPOLE = 8
        }


        public static string SessionTypeToString(AcSessionType sessionType)
        {
            switch (sessionType)
            {
                case AcSessionType.AC_UNKNOWN: return "Unknown";
                case AcSessionType.AC_PRACTICE: return "Practice";
                case AcSessionType.AC_QUALIFY: return "Qualify";
                case AcSessionType.AC_RACE: return "Race";
                case AcSessionType.AC_HOTLAP: return "Hotlap";
                case AcSessionType.AC_TIME_ATTACK: return "Time attack";
                case AcSessionType.AC_DRIFT: return "Drift";
                case AcSessionType.AC_DRAG: return "Drag";
                case AcSessionType.AC_HOTSTINT: return "Hotstint";
                case AcSessionType.AC_HOTLAPSUPERPOLE: return "Hotlap superpole";

                default: return sessionType.ToString();
            }
        }

        public enum AcFlagType : int
        {
            AC_NO_FLAG,
            AC_BLUE_FLAG,
            AC_YELLOW_FLAG,
            AC_BLACK_FLAG,
            AC_WHITE_FLAG,
            AC_CHECKERED_FLAG,
            AC_PENALTY_FLAG,
            AC_GREEN_FLAG,
            AC_BLACK_FLAG_WITH_ORANGE_CIRCLE,

        }

        public static string FlagTypeToString(AcFlagType flagType)
        {
            switch (flagType)
            {
                case AcFlagType.AC_NO_FLAG: return "Green";
                case AcFlagType.AC_BLUE_FLAG: return "Blue";
                case AcFlagType.AC_YELLOW_FLAG: return "Yellow";
                case AcFlagType.AC_BLACK_FLAG: return "Black";
                case AcFlagType.AC_WHITE_FLAG: return "White";
                case AcFlagType.AC_CHECKERED_FLAG: return "Checkered";
                case AcFlagType.AC_PENALTY_FLAG: return "Penalty";
                case AcFlagType.AC_GREEN_FLAG: return "Green";
                case AcFlagType.AC_BLACK_FLAG_WITH_ORANGE_CIRCLE: return "Orange";

                default: return flagType.ToString();
            }
        }

        public enum PenaltyShortcut : int
        {
            None,
            DriveThrough_Cutting,
            StopAndGo_10_Cutting,
            StopAndGo_20_Cutting,
            StopAndGo_30_Cutting,
            Disqualified_Cutting,
            RemoveBestLaptime_Cutting,

            DriveThrough_PitSpeeding,
            StopAndGo_10_PitSpeeding,
            StopAndGo_20_PitSpeeding,
            StopAndGo_30_PitSpeeding,
            Disqualified_PitSpeeding,
            RemoveBestLaptime_PitSpeeding,

            Disqualified_IgnoredMandatoryPit,

            PostRaceTime,
            Disqualified_Trolling,
            Disqualified_PitEntry,
            Disqualified_PitExit,
            Disqualified_WrongWay,

            DriveThrough_IgnoredDriverStint,
            Disqualified_IgnoredDriverStint,

            Disqualified_ExceededDriverStintLimit,
        };

        public enum AcTrackGripStatus : int
        {
            Green,
            Fast,
            Optimum,
            Greasy,
            Damp,
            Wet,
            Flooded
        };

        public enum AcRainIntensity : int
        {
            No_Rain,
            Drizzle,
            Light_Rain,
            Medium_Rain,
            Heavy_Rain,
            Thunderstorm,
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode), Serializable]
        public class PageFileGraphics
        {
            public int PacketId;
            public AcStatus Status;
            public AcSessionType SessionType;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string CurrentTime;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string LastTime;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string BestTime;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string Split;

            public int CompletedLaps;
            public int Position;
            public int CurrentTimeMs;
            public int LastTimeMs;
            public int BestTimeMs;
            public float SessionTimeLeft;
            public float DistanceTraveled;

            [MarshalAs(UnmanagedType.Bool)]
            public bool IsInPits;

            public int CurrentSectorIndex;
            public int LastSectorTime;
            public int NumberOfLaps;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string TyreCompound;

            public float ReplayTimeMultiplier;
            public float NormalizedCarPosition;

            public int ActiveCars;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public Vector3[] CarCoordinates;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public int[] CarIds;

            public int PlayerCarID;

            public float PenaltyTime;
            public AcFlagType Flag;

            public PenaltyShortcut PenaltyType;

            [MarshalAs(UnmanagedType.Bool)]
            public bool IdealLineOn;

            [MarshalAs(UnmanagedType.Bool)]
            public bool IsInPitLane;
            public float SurfaceGrip;

            [MarshalAs(UnmanagedType.Bool)]
            public bool MandatoryPitDone;


            public static readonly int Size = Marshal.SizeOf(typeof(PageFileGraphics));
            public static readonly byte[] Buffer = new byte[Size];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode), Serializable]
        public struct StructVector3
        {
            public float X;
            public float Y;
            public float Z;

            public readonly override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode), Serializable]
        public class PageFilePhysics
        {
            public int PacketId;
            public float Gas;
            public float Brake;
            public float Fuel;
            public int Gear;
            public int Rpms;
            public float SteerAngle;
            public float SpeedKmh;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] Velocity;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] AccG;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] WheelSlip;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] WheelLoad;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] WheelPressure;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] WheelAngularSpeed;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreWear;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreDirtyLevel;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreCoreTemperature;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] CamberRad;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] SuspensionTravel;

            public float Drs;
            public float TC;
            public float Heading;
            public float Pitch;
            public float Roll;

            public float CgHeight;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            public float[] CarDamage;

            public int NumberOfTyresOut;

            [MarshalAs(UnmanagedType.Bool)]
            public bool PitLimiterOn;
            public float Abs;

            public float KersCharge;

            public float KersInput;

            [MarshalAs(UnmanagedType.Bool)]
            public bool AutoShifterOn;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public float[] RideHeight;

            public float TurboBoost;

            public float Ballast;
            public float AirDensity;

            public float AirTemp;
            public float RoadTemp;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] LocalAngularVelocity;

            public float finalFF;

            public float PerformanceMeter;
            public int EngineBrake;
            public int ErsRecoveryLevel;
            public int ErsPowerLevel;
            public int ErsHeatCharging;
            public int ErsIsCharging;
            public float KersCurrentKJ;

            [MarshalAs(UnmanagedType.Bool)]
            public bool DrsAvailable;

            [MarshalAs(UnmanagedType.Bool)]
            public bool DrsEnabled;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] BrakeTemperature;

            public float Clutch;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreTempI;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreTempM;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreTempO;

            [MarshalAs(UnmanagedType.Bool)]
            public bool IsAiControlled;


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public StructVector3[] TyreContactPoint;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public StructVector3[] TyreContactNormal;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public StructVector3[] TyreContactHeading;

            public float BrakeBias;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public float[] LocalVelocity;
        
            public static readonly int Size = Marshal.SizeOf(typeof(PageFilePhysics));
            public static readonly byte[] Buffer = new byte[Size];
        };

        [StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Unicode), Serializable]
        public class PageFileStatic
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string SharedMemoryVersion;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 15)]
            public string AssettoCorsaVersion;

            public int NumberOfSessions;
            public int NumberOfCars;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string CarModel;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string Track;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string PlayerName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string PlayerSurname;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string PlayerNickname;

            public int SectorCount;

            // car static info
            public float MaxTorque;
            public float MaxPower;
            public int MaxRpm;
            public float MaxFuel;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] SuspensionMaxTravel;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public float[] TyreRadius;

            public float MaxTurboBoost;

            public float AirTemperature;

            public float RoadTemperature;

            [MarshalAs(UnmanagedType.Bool)]
            public bool PenaltiesEnabled;
            public float AidFuelRate;
            public float AidTireRate;
            public float AidMechanicalDamage;
            [MarshalAs(UnmanagedType.Bool)]
            public bool AidAllowTyreBlankets;
            public float AidStability;
            [MarshalAs(UnmanagedType.Bool)]
            public bool AidAutoClutch;
            [MarshalAs(UnmanagedType.Bool)]
            public bool AidAutoBlip;

            [MarshalAs(UnmanagedType.Bool)]
            public bool HasDRS;

            [MarshalAs(UnmanagedType.Bool)]
            public bool HasERS;

            [MarshalAs(UnmanagedType.Bool)]
            public bool HasKERS;
            public float KersMaxJoules;
            public int EngineBrakeSettingsCount;
            public int ErsPowerControllerCount;
            public float TrackSplineLength;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string TrackConfiguration;
            public float ErsMaxJ;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsTimedRace;
            [MarshalAs(UnmanagedType.Bool)]
            public bool HasExtraLap;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
            public string CarSkin;
            public int ReversedGridPositions;

            public int PitWindowStart;
            public int PitWindowEnd;

            public static readonly int Size = Marshal.SizeOf(typeof(PageFileStatic));
            public static readonly byte[] Buffer = new byte[Size];
        };

        public static PageFileGraphics ReadGraphicsPageFile(bool fromCache = false)
        {
            if (fromCache) return GraphicsPage;
            return GraphicsPage = MemoryMappedFile.CreateOrOpen(graphicsMap, sizeof(byte), MemoryMappedFileAccess.ReadWrite).ToStruct<PageFileGraphics>(PageFileGraphics.Buffer);
        }

        public static PageFileStatic ReadStaticPageFile(bool fromCache = false)
        {
            if (fromCache) return StaticPage;
            return StaticPage = MemoryMappedFile.CreateOrOpen(staticMap, sizeof(byte), MemoryMappedFileAccess.ReadWrite).ToStruct<PageFileStatic>(PageFileStatic.Buffer);
        }

        public static PageFilePhysics ReadPhysicsPageFile(bool fromCache = false)
        {
            if (fromCache) return PhysicsPage;
            return PhysicsPage = MemoryMappedFile.CreateOrOpen(physicsMap, sizeof(byte), MemoryMappedFileAccess.ReadWrite).ToStruct<PageFilePhysics>(PageFilePhysics.Buffer);
        }
    }
}
