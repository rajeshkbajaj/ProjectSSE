#ifndef VIKINGCONSOLEDRIVER_H
#define VIKINGCONSOLEDRIVER_H
/*
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
*/

#ifdef VIKINGCONSOLEDRIVER_EXPORTS
#define VIKINGCONSOLEDRIVER_API __declspec(dllexport)
#else
#define VIKINGCONSOLEDRIVER_API __declspec(dllimport)
#endif

#include<TCHAR.H>

#ifdef __cplusplus

#include "IVikingConsoleDriverImpl.h"
#include "ITokenImpl.h"
#include "IRemoteFileOperations.h"  

typedef int socklen_t;




extern "C" {

    struct VikingConsoleDriver;
    struct VikingConsoleToken;
    struct RemoteFileSocketServer;
    struct RemoteFileIO;
#else 

    typedef struct tagVikingConsoleDriver VikingConsoleDriver;
    typedef struct tagVikingConsoleToken  VikingConsoleToken;
    typedef struct tagRemoteFileSocketServer RemoteFileSocketServer;
    typedef struct tagRemoteFileIO  RemoteFileIO;
#endif


#pragma warning( push )
#pragma warning( disable : 4248 )


    VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_finish(VikingConsoleDriver *pVikingConsoleDriver);
    VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_setDelay(VikingConsoleDriver *pVikingConsoleDriver, int msec);
    VIKINGCONSOLEDRIVER_API VikingConsoleDriver * VikingConsoleDriver_init(wchar_t * path);

	//Download service management.
	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_setDownloadServerWorkingPath(VikingConsoleDriver *pVikingConsoleDriver, wchar_t * path);
    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_startDownloadServer(VikingConsoleDriver *pVikingConsoleDriver);
    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_stopDownloadServer(VikingConsoleDriver *pVikingConsoleDriver);
	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_getDownloadServerState(VikingConsoleDriver *pVikingConsoleDriver);

	//Cleanup all handles and dispose of pending IO operations.
    VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_reset(VikingConsoleDriver *pVikingConsoleDriver);

	//A Trigger is used to start a download.  A Trigger will only work if DownloadServer is started.
	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_enableTrigger(VikingConsoleDriver *pVikingConsoleDriver);
    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_disableTrigger(VikingConsoleDriver *pVikingConsoleDriver);
    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_getTriggerState(VikingConsoleDriver *pVikingConsoleDriver);
	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_oneShotTrigger(VikingConsoleDriver *pVikingConsoleDriver);
	VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_IsDeviceReady(VikingConsoleDriver *pVikingConsoleDriver);
	VIKINGCONSOLEDRIVER_API void VikingConsoleDriver_listenForConfig(VikingConsoleDriver *pVikingConsoleDriver);
	//Parsing functions
	VIKINGCONSOLEDRIVER_API VikingConsoleToken *  VikingConsoleDriver_getNextToken(VikingConsoleDriver *pVikingConsoleDriver);
    VIKINGCONSOLEDRIVER_API const char *VikingConsoleDriver_getValue(VikingConsoleToken *pVikingConsoleToken, int val);
    VIKINGCONSOLEDRIVER_API int VikingConsoleDriver_getLength(VikingConsoleToken * pVikingConsoleToken);



#pragma warning( pop ) 

#ifdef __cplusplus
}

#endif 

#endif
