//----------------------------------------------------------------------------
//            Copyright (c) 2012 Covidien, Inc.
//
// This software is copyrighted by and is the sole property of Covidien. This
// is a proprietary work to which Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Covidien.
//----------------------------------------------------------------------------

#include "VikingConsoleDriver.h"
#include "VikingConsoleDriverImpl.h"

//extern "C"
//{

    //-------------------------------------
    // M A C R O S
    //-------------------------------------
#ifndef min
#define min(X,Y)  (((X)<(Y))?(X):(Y))
#endif



    ////////////////////
    /// Exported Functions
    /// Interace 
    ///////////////////

    VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_finish(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return;
        }
        ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->finish();
    }



    VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_setDelay(VikingConsoleDriver *pVikingConsoleDriver, int msec)
    {
        if(pVikingConsoleDriver == 0)
        {
            return;
        }

        ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->setDelay(msec);
    }


    VIKINGCONSOLEDRIVER_API VikingConsoleDriver * VikingConsoleDriver_init(wchar_t * path)
    {
        return ((VikingConsoleDriver *)VikingConsoleDriverImpl::init(path));
    }

    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_startDownloadServer(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return -1;
        }


        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->startDownloadServer();
    }


	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_enableTrigger(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return -1;
        }


        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->enableTrigger();
    }


	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_getTriggerState(VikingConsoleDriver *pVikingConsoleDriver)
	{
        if(pVikingConsoleDriver == 0)
        {
            return -1;
        }

        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->getTriggerState();
	}


	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_setDownloadServerWorkingPath(VikingConsoleDriver *pVikingConsoleDriver, wchar_t * path)
	{
		if(pVikingConsoleDriver == 0)
        {
            return -1;
        }

		return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->setDownloadServerWorkingPath(path);
	}


	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_disableTrigger(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return -1;
        }


        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->disableTrigger();
    }



	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_oneShotTrigger(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return -1;
        }


        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->oneShotTrigger();
    }

	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_IsDeviceReady(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return false;
        }
		///todo remove
		bool val = ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->isDeviceReady();
        return ((val == true)? 1 : 0);
    }


	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_getDownloadServerState(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return -1;
        }


        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->getDownloadServerState();
    }	
	



    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_stopDownloadServer(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return 0;
        }

        return ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->stopDownloadServer();
    }


    VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_reset(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return;
        }

        ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->reset();
    }


    VIKINGCONSOLEDRIVER_API VikingConsoleToken *  VikingConsoleDriver_getNextToken(VikingConsoleDriver *pVikingConsoleDriver)
    {
        if(pVikingConsoleDriver == 0)
        {
            return 0;
        }

        return (VikingConsoleToken *) (((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->getNextToken());
    }


    VIKINGCONSOLEDRIVER_API const char *VikingConsoleDriver_getValue(VikingConsoleToken *pVikingConsoleToken, int val)
    {
        if(pVikingConsoleToken == 0)
        {
            return "";
        }

        ITokenImpl *pToken = (ITokenImpl *)pVikingConsoleToken;

        return pToken->getValue(val);
    }


    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_getLength(VikingConsoleToken * pVikingConsoleToken)
    {
        if(pVikingConsoleToken == 0)
        {
            return 0;
        }

        ITokenImpl *pToken = (ITokenImpl *)pVikingConsoleToken;

        return pToken->getLength();
    }

	VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_listenForConfig(VikingConsoleDriver *pVikingConsoleDriver)
	{
		if(pVikingConsoleDriver == 0)
			return;

		 ((IVikingConsoleDriverImpl *)pVikingConsoleDriver)->listenForConfig();
	}
//}









