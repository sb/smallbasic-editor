// <copyright file="SuperBasicEnv.cs" company="2018 Omar Tawfik">
// Copyright (c) 2018 Omar Tawfik. All rights reserved. Licensed under the MIT License. See LICENSE file in the project root for license information.
// </copyright>

namespace SuperBasic.Utilities
{
    public static class SuperBasicEnv
    {
        public static bool IsBuildingForDesktop
        {
            get
            {
#if IsBuildingForDesktop
                return true;
#else
                return false;
#endif
            }
        }
    }
}
