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

#include "SoftwareOptionsKeyDriver.h"
#include "SoftwareOptionsKey.h"

//using namespace SoftwareOptions;

SOFTWAREOPTIONSKEYDRIVER_API SoftwareOptionsKeyDriver * SoftwareOptionsKey_getInstance()
{
	return ((SoftwareOptionsKeyDriver *) new SoftwareOptionsKey());
}

SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_deleteInstance(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return;

	delete pSoftwareOptionsKeyDriver;
}

SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_updateKey(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, wchar_t* encryptedKey, wchar_t* ventSN)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return;

	std::wstring wKey(encryptedKey);
	std::wstring wSN(ventSN);

	std::string encKey;
	encKey.assign(wKey.begin(), wKey.end()); 

	std::string ventSerialNumber;
	ventSerialNumber.assign(wSN.begin(), wSN.end()); 

	((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->updateKey(encKey, ventSerialNumber);
}

SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_setOptionState(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id, int state)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return;

	((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->setOptionState((SoftwareOptionsKey::OptionId)id, (state!=0));

}

SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getOptionState(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return 0;

	if(((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->getOptionState((SoftwareOptionsKey::OptionId)id) == true)
		return 1 ;
	else
		return 0;
}

SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_setKeyExpiryDate(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id, int day, int month, int year)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return;

	((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->setKeyExpiryDate((SoftwareOptionsKey::OptionId)id, (Uint8)day, (Uint8)month, (Uint8)year);
}

SOFTWAREOPTIONSKEYDRIVER_API void SoftwareOptionsKey_getEncryptedKey(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, char* encryptedKey, int length)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return;

	std::string encKey = ((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->getEncryptedKey();

	if(length > encKey.length())
	{
		memcpy(encryptedKey, encKey.c_str(), encKey.length());
		memset(encryptedKey+encKey.length(), 0, 1);
	}
}

SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getKeyExpiryDay(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return 0;

	Uint8 day;
	Uint8 month;
	Uint8 year;

	((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->getKeyExpiryDate((SoftwareOptionsKey::OptionId)id, day, month, year);
	return day;
}

SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getKeyExpiryMonth(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return 0;
	
	Uint8 day;
	Uint8 month;
	Uint8 year;

	((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->getKeyExpiryDate((SoftwareOptionsKey::OptionId)id, day, month, year);
	return month;
}

SOFTWAREOPTIONSKEYDRIVER_API int SoftwareOptionsKey_getKeyExpiryYear(SoftwareOptionsKeyDriver *pSoftwareOptionsKeyDriver, int id)
{
	if(pSoftwareOptionsKeyDriver == 0)
		return 0;

	Uint8 day;
	Uint8 month;
	Uint8 year;

	((SoftwareOptionsKey*)pSoftwareOptionsKeyDriver)->getKeyExpiryDate((SoftwareOptionsKey::OptionId)id, day, month, year);
	return ((int)year);
}
