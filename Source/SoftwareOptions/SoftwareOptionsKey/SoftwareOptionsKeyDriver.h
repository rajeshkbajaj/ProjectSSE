#ifndef SOFTWAREOPTIONSKEYDRIVER_H
#define SOFTWAREOPTIONSKEYDRIVER_H
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

#ifdef SOFTWAREOPTIONSKEYDRIVER_EXPORTS
#define SOFTWAREOPTIONSKEYDRIVER_API __declspec(dllexport)
#else
#define SOFTWAREOPTIONSKEYDRIVER_API __declspec(dllimport)
#endif

#include<TCHAR.H>

#ifdef __cplusplus

#include "SoftwareOptionsKey.h"

extern "C" {
    struct SoftwareOptionsKeyDriver;
	enum OptionId;
#else 
    typedef struct tagVikingConsoleDriver VikingConsoleDriver;
    typedef struct tagVikingConsoleToken  VikingConsoleToken;
    typedef struct tagRemoteFileSocketServer RemoteFileSocketServer;
    typedef struct tagRemoteFileIO  RemoteFileIO;
#endif


#pragma warning( push )
#pragma warning( disable : 4248 )

    SOFTWAREOPTIONSKEYDRIVER_API SoftwareOptionsKeyDriver * SoftwareOptionsKey_getInstance();
	SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_deleteInstance(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver);
	SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_updateKey(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, wchar_t* encryptedKey, wchar_t* ventSN);
	SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_setOptionState(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id, int state);
	SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getOptionState(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id);
	SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_setKeyExpiryDate(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id, int day, int month, int year);
	SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_getEncryptedKey(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, char* encryptedKey, int length);
	SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getKeyExpiryDay(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id);
	SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getKeyExpiryMonth(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id);
	SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getKeyExpiryYear(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id);

#pragma warning( pop ) 

#ifdef __cplusplus
}

#endif 

#endif