﻿using AOPGiris.Aspect.Abstract;
using AOPGiris.Aspect.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOPGiris.Aspect.JoinPoints
{
    public interface IBeforeVoidParamAspect : IAspect
    {
        void OnBefore(RealTypeResponseArgument param);
    }
}
