﻿using DowJones.Managers.Core;

namespace DowJones.Managers.Core
{
    public interface IServicePartResult<TPackage> where TPackage : IPackage
    {
        string Identifier
        {
            get;
            set;
        }

        long ReturnCode
        {
            get;
            set;
        }

        long ElapsedTime
        {
            get;
            set;
        }

        string StatusMessage
        {
            get;
            set;
        }

        string PackageType
        {
            get;
            set;
        }

        TPackage Package
        {
            get;
            set;
        }
    }
}