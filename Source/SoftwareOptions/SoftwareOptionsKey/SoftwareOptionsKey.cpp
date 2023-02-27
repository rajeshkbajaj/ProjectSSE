#include "SoftwareOptionsKey.h"
#include <vector>
#include <sstream>
#include <iostream>
#include <string.h>

#if defined (_WIN32) || defined (_WIN64)
#define strncasecmp _strnicmp
#else
#include <strings.h>
#endif

static const unsigned int PACKED_VENT_SERIAL_NUMBER_LEN = 5;
static const unsigned int UNPACKED_VENT_SERIAL_NUMBER_LEN = 10;
void dumpBufferHex(const Uint8* buffer, Uint32 len, const std::string& prefix = "", const std::string delim = ",")
{
    std::cout << prefix << std::hex;
    for(Uint32 i=0; i<len;i++)
    {
        std::cout << (Uint32)buffer[i] << delim;
    }
    std::cout << std::dec << std::endl;
}
void dumpBufferHex(std::vector<Uint8>&buffer, const std::string& prefix = "", const std::string delim = ",")
{
    dumpBufferHex(&buffer[0], buffer.size(), prefix, delim);
}

SoftwareOptionsKey* SoftwareOptionsKey::_instance = NULL;

SoftwareOptionsKey::ConfigurationCode_::ConfigurationCode_()
{
    memset(this, 0, sizeof(SoftwareOptionsKey::ConfigurationCode_));
}
const std::string SoftwareOptionsKey::ConfigurationCode_::getAsString() const
{
    std::stringstream stream;
    stream << std::hex << "____ConfigurationCode____ " << "\r\n";
    stream << "Customer Options: " << customerOptions << "\r\n";
    const std::string OptionNames[] ={ "RESERVED1", "ATC", "NEO_MODE", "VTPC", 
        "RESP_MECH", "TRENDING", "NEO_MODE_UPDATE", "NEO_MODE_ADVANCED", 
        "NEO_MODE_LOCKOUT", "BILEVEL", "PAV", "IE_SYNC_ASYNC", "LEAK_COMP",
        "ESM", "CAPNOGRAPHY", ""};
    for(Uint32 i=0; OptionNames[i] != ""; i++)
    {
        const char* state = (customerOptions & (1 << i))? "ENABLED":"DISABLED";
        stream << OptionNames[i] << " : " << state << "\r\n";
    }

    const std::string KeyNames[] = { "INVALID", "NORMAL", "ENGINEERING",
        "SALES", "MANUFACTURING", "INVALID-NUM" };
    
    stream << "Key Type: " << KeyNames[keyType] << std::dec << "\r\n";
    stream << "Expiration Date: " << (Uint32)expirationMonth << "/" << (Uint32)expirationDay 
        << "/" << (Uint32)expirationYear+2000 << "\r\n";
    return stream.str();
}
bool SoftwareOptionsKey::ConfigurationCode_::getToByteStream(Uint8* buffer, Uint32 len) const
{
    //For ConfigurationCode_, there is no padding problems at the moment (i.e. as of 7/24/2013.
    //(a 4-byte integer, followed by 4 single byte characters.. totaling 8 bytes). So we can 
    //just copy the whole struct instance to the buffer. However, I am doing a member-by-member 
    //copy just to be consistent, and to make it easier if a member is added/removed in the future.
    if(len < getUnpaddedSize())
    {
        return false;
    }
    Uint8* dstPtr = buffer;
    //This code will need further testing depending on the vent's architecture. The current implementation
    //is meant for a vent with big endian format (like the powerpc on viking)
    *dstPtr = (customerOptions & 0xff000000) >> 24; dstPtr++;
    *dstPtr = (customerOptions & 0xff0000) >> 16; dstPtr++;
    *dstPtr = (customerOptions & 0xff00) >> 8; dstPtr++;
    *dstPtr = customerOptions & 0xff; dstPtr++;
    *dstPtr = keyType; dstPtr++;
    *dstPtr = expirationMonth; dstPtr++;
    *dstPtr = expirationDay; dstPtr++;
    *dstPtr = expirationYear; dstPtr++;
    return true;
}
bool SoftwareOptionsKey::ConfigurationCode_::setFromByteStream(const Uint8* buffer, Uint32 len)
{
    if(len < getUnpaddedSize())
    {
        return false;
    }
    const Uint8* srcPtr = buffer;
    //See the note on big endian storage earlier (required for the PowerPC architecture used in Viking)
    customerOptions = ((*srcPtr) << 24)| (*(srcPtr+1) << 16) | (*(srcPtr+2) << 8) | *(srcPtr+3);
    srcPtr += 4;
    keyType = (KeyType)(*srcPtr); srcPtr++;
    expirationMonth = *srcPtr; srcPtr++;
    expirationDay = *srcPtr; srcPtr++;
    expirationYear = *srcPtr; srcPtr++;
    return true;

}
Uint32 SoftwareOptionsKey::ConfigurationCode_::getUnpaddedSize() const
{
    //The compiler adds padding to align struct's fields with word boundaries.
    //The sizeof() operator returns the size allocated by the compiler, not the 
    //sum of sizes for each member. This API returns the sum of sizes of all elements.
    return sizeof(customerOptions) + sizeof(keyType) + sizeof(expirationMonth) +
                            sizeof(expirationDay) + sizeof(expirationYear);
}
SoftwareOptionsKey::PlainSoftwareKey::PlainSoftwareKey()
{
    memset(this, 0, sizeof(SoftwareOptionsKey::PlainSoftwareKey));
}

const std::string SoftwareOptionsKey::PlainSoftwareKey::getAsString() const
{
    std::stringstream stream;
    stream << std::hex << "______Software Key______" << "\r\n";
    stream << "Validation Code: " << validationCode << "\r\n";
    stream << "Packed Vent SN: ";
    for(Uint32 i=0;i<sizeof(ventSerialNumber);i++)
    {
        //stream << (Uint32)ventSerialNumber[i];
        //write nibbles separately so as to not lose any '0's.
        FancyByte fb(ventSerialNumber[i]);
        stream << (Uint32)fb.msnibble() << (Uint32)fb.lsnibble();
    }
    stream << "\r\n";
    stream << "Programmer Mode: " << (Uint32) programmerMode << std::dec << "\r\n";
    stream << configurationCode.getAsString();

    return stream.str();
}
bool SoftwareOptionsKey::PlainSoftwareKey::getToByteStream(Uint8* buffer, Uint32 len) const
{
    //For ConfigurationCode_, there is no padding problems at the moment (i.e. as of 7/24/2013.
    //(a 4-byte integer, followed by 4 single byte characters.. totaling 8 bytes). So we can 
    //just copy the whole struct instance to the buffer. However, I am doing a member-by-member 
    //copy just to be consistent, and to make it easier if a member is added/removed in the future.
    if(len < getUnpaddedSize())
    {
        return false;
    }
    Uint8* dstPtr = buffer;
    //*((Uint32*)dstPtr) = validationCode;  dstPtr += 4;
    //See the note about big endian format on the vent.
    *dstPtr = (validationCode & 0xff000000) >> 24; dstPtr++;
    *dstPtr = (validationCode & 0xff0000) >> 16; dstPtr++;
    *dstPtr = (validationCode & 0xff00) >> 8; dstPtr++;
    *dstPtr = validationCode & 0xff; dstPtr++;
    memmove(dstPtr, ventSerialNumber, PACKED_VENT_SERIAL_NUMBER_LEN); dstPtr += 5;
    configurationCode.getToByteStream(dstPtr, len-UNPACKED_VENT_SERIAL_NUMBER_LEN);  //4+5 + 1 (for programmer mode later)
    dstPtr += configurationCode.getUnpaddedSize();
    *dstPtr = programmerMode; dstPtr++;
    return true;
}
bool SoftwareOptionsKey::PlainSoftwareKey::setFromByteStream(const Uint8* buffer, Uint32 len)
{
    if(len < getUnpaddedSize())
    {
        return false;
    }
    const Uint8* srcPtr = buffer;
    //validationCode = *((Uint32*)srcPtr); srcPtr += 4;
    //See the note earlier about big endian format
    validationCode = ((*srcPtr) << 24)| (*(srcPtr+1) << 16) | (*(srcPtr+2) << 8) | *(srcPtr+3);
    srcPtr += 4;
    memmove(ventSerialNumber, srcPtr, PACKED_VENT_SERIAL_NUMBER_LEN); srcPtr += PACKED_VENT_SERIAL_NUMBER_LEN;
    configurationCode.setFromByteStream(srcPtr, len-UNPACKED_VENT_SERIAL_NUMBER_LEN);  //4+5 + 1 (for programmer mode later)
    srcPtr += configurationCode.getUnpaddedSize();
    programmerMode = *srcPtr; srcPtr++;

    return true;

}

Uint32 SoftwareOptionsKey::PlainSoftwareKey::getUnpaddedSize() const
{
    return sizeof(validationCode) + sizeof(ventSerialNumber) 
        + configurationCode.getUnpaddedSize() + sizeof(programmerMode);
}

bool SoftwareOptionsKey::PlainSoftwareKey::merge(const SoftwareOptionsKey::PlainSoftwareKey& fromKey)
{
    bool ret = false;
    //Check if both keys are meant for the same vent.. if not return.
    for(Uint8 i=0; i<sizeof(ventSerialNumber); i++)
        if(ventSerialNumber[i] != fromKey.ventSerialNumber[i])
            return ret;
    //No need to check validation code (will be different for two keys).. besides, the merged one will
    //need to recompute its validation code anyway
    //Ignore 'programmer mode' as well. It is unused.
    Uint32 newCustOptions = configurationCode.customerOptions | fromKey.configurationCode.customerOptions;
    if(newCustOptions & (1u << (Uint32)ESM))
    {
        //The ESM is enabled in the combined key.. 
        //Pick the longest of time remaining from the two keys
        //First check if the ESM is enabled in the new key
        if(fromKey.configurationCode.customerOptions & (1u << (Uint32)ESM))
        {
            //ESM was enabled in the new key.. check if it is also enabled in the current key
            if(configurationCode.customerOptions & (1u << (Uint32)ESM))
            {
                //ESM was enabled in the current key as well.. pick the one that expires last
                Uint32 currentExpiry = (configurationCode.expirationYear << 16) | 
                                            (configurationCode.expirationMonth << 8) |
                                                (configurationCode.expirationDay);
                Uint32 newExpiry = (fromKey.configurationCode.expirationYear << 16) | 
                                            (fromKey.configurationCode.expirationMonth << 8) |
                                                (fromKey.configurationCode.expirationDay);
                std::cout << std::hex << "Expiry dates: Current " << currentExpiry << ", New " << newExpiry << std::dec << std::endl;
                if(currentExpiry == 0 || newExpiry == 0)
                {
                    std::cout << "One key has permanent ESM..." << std::endl;
                    configurationCode.expirationDay = 0;
                    configurationCode.expirationMonth = 0;
                    configurationCode.expirationYear = 0;
                }
                else if(newExpiry > currentExpiry)
                {
                    //The new expiry date is after the current expiry date
                    std::cout << "ESM new longer than current.. Using it..." << std::endl;
                    configurationCode.expirationDay = fromKey.configurationCode.expirationDay;
                    configurationCode.expirationMonth = fromKey.configurationCode.expirationMonth;
                    configurationCode.expirationYear = fromKey.configurationCode.expirationYear;
                }
                else
                {
                    //The current key has a longer expiry date.. use the current settings.
                }
            }
            else
            {
                //ESM wasnt enabled on current key.. just use the expiry date from new key
                std::cout << "ESM only in new key. Using it..." << std::endl;
                configurationCode.expirationDay = fromKey.configurationCode.expirationDay;
                configurationCode.expirationMonth = fromKey.configurationCode.expirationMonth;
                configurationCode.expirationYear = fromKey.configurationCode.expirationYear;

            }
        }
        else
        {
            //The new key had no ESM option.. just use the current settings.
        }


    }
    configurationCode.customerOptions = newCustOptions;
    //Override current key type with the new one.
    configurationCode.keyType = fromKey.configurationCode.keyType;
    return ret;
}

SoftwareOptionsKey* SoftwareOptionsKey::getInstance()
{
    if(_instance == NULL)
        _instance = new SoftwareOptionsKey();

    return _instance;
}
SoftwareOptionsKey::SoftwareOptionsKey()
    :_initialized(false)
{
    _init();
}

bool SoftwareOptionsKey::updateKey(const std::string& encryptedKey, const std::string& ventSN)
{
    bool ret = false;
    if(!_initialized)
    {
        //This is the first key being set.. just initialize the internal key
        ret = _decodeToKey(encryptedKey, ventSN, _key);
        if(!ret)
        {
            //the decoding didnt work.. just initialize back to defaults.
            _init();
        }
        else
        {
            _initialized = true;
        }
    }
    else
    {
        //This key is already initialized.. the attempt now is to add another key (like a temporary
        //key). We need to decode and merge with the currently set key
        std::cout << "Adding second key..." << std::endl;
        PlainSoftwareKey newKey;
        ret = _decodeToKey(encryptedKey, ventSN, newKey);
        if(ret)
        {
            //Now merge the new key with current
            _key.merge(newKey);
            _key.validationCode = _getValidationCode(_key.ventSerialNumber, 
                    sizeof(_key.ventSerialNumber), _key.configurationCode);
        }
        else
        {
            //There is nothing really to do here.. for now. The 'ret' already
            //reflects an error scenario
        }
    }
    return ret;
}

bool SoftwareOptionsKey::getOptionState(OptionId id) const
{
    bool ret = ((_key.configurationCode.customerOptions & (1u << id)) != 0);
    if(ret == true && id == ESM)
    {
        TimeStamp now; //Include Timestamp.hh file to get the current timestamp
        Uint32 currentDate = 0;
        Uint32 expiryDate = 0;

        currentDate = ((now.getYear() - 2000) << 16) |
                        (now.getMonth() << 8) | 
                        now.getDayOfMonth();

        //Create a date "number" 00YYMMDD to make comparison easier
        expiryDate = (_key.configurationCode.expirationYear << 16) |
                        (_key.configurationCode.expirationMonth << 8) |
                        _key.configurationCode.expirationDay;

        /*Check if current date is past the expiry date. The expiry date has a special
        * value of 0/0/0 (ie. 0 in number form) that says 'no expiry'*/
        if(currentDate > expiryDate && expiryDate != 0)
        {
            ret  = false;
        }
    }

	return ret;
}
const std::string SoftwareOptionsKey::getVentSerialNumber() const
{
    std::string vsn;
    _unpackBCHString(_key.ventSerialNumber, PACKED_VENT_SERIAL_NUMBER_LEN, vsn);
    return vsn;
}
SoftwareOptionsKey::KeyType SoftwareOptionsKey::getKeyType() const
{
    return _key.configurationCode.keyType;
}

const std::string SoftwareOptionsKey::getEncryptedKey() const
{
    std::string ret;
    std::vector<Uint8> buffer(_key.getUnpaddedSize());
    _key.getToByteStream(&buffer[0], buffer.size());
    _encrypt((Uint8*)&buffer[0], buffer.size(), ret);
    return ret;
}
const std::string SoftwareOptionsKey::getExplanatoryString() const
{
    return _key.getAsString();
}

void SoftwareOptionsKey::setOptionState(OptionId id, bool state)
{
    if(state)
        _key.configurationCode.customerOptions |= (1u << id);
    else
        _key.configurationCode.customerOptions &= ~(1u << id);
    //Update the validation code, because the option has changed
    _key.validationCode = _getValidationCode(_key.ventSerialNumber, 
            sizeof(_key.ventSerialNumber), _key.configurationCode);
}

bool SoftwareOptionsKey::setVentSerialNumber(const std::string& vsn)
{
    bool ret = false;
    if(vsn.length() == UNPACKED_VENT_SERIAL_NUMBER_LEN)
    {
        memset(_key.ventSerialNumber, 0, sizeof(_key.ventSerialNumber));
        ret = _convertToPackedBCH(vsn, _key.ventSerialNumber);
    }
    if(ret)
    {
        //Update the validation code, because the VSN has been updated
        _key.validationCode = _getValidationCode(_key.ventSerialNumber, 
                sizeof(_key.ventSerialNumber), _key.configurationCode);
    }
    return ret;
}

void SoftwareOptionsKey::setKeyType(KeyType type)
{
    _key.configurationCode.keyType = type;
    //Update the validation code, because the VSN has been updated
    _key.validationCode = _getValidationCode(_key.ventSerialNumber, 
            sizeof(_key.ventSerialNumber), _key.configurationCode);
}

bool SoftwareOptionsKey::setKeyExpiryDate(OptionId id, Uint8 day, Uint8 month, Uint8 year)
{
    bool ret = false;
    if(id == ESM)
    {
	    _key.configurationCode.expirationYear = year;
	    _key.configurationCode.expirationMonth = month;
	    _key.configurationCode.expirationDay = day;
        //Update the validation code, because the VSN has been updated
        _key.validationCode = _getValidationCode(_key.ventSerialNumber, 
                sizeof(_key.ventSerialNumber), _key.configurationCode);
        ret = true;
    }
	return ret;
}
void SoftwareOptionsKey::getKeyExpiryDate(OptionId id, Uint8& day, Uint8& month, Uint8& year)
{
    if(id == ESM)
    {
        year = _key.configurationCode.expirationYear;
        month = _key.configurationCode.expirationMonth;
        day = _key.configurationCode.expirationDay;
    }
    else
        day = month = year = 0;
}

void SoftwareOptionsKey::_init()
{
    memset(&_key, 0, sizeof(PlainSoftwareKey));
    //Set the default type to NORMAL
    setKeyType(NORMAL);
    //Enable the 'always-on' options. Eventually we may remove these option
    //ids from the code.
    setOptionState(ATC, true);
    setOptionState(VTPC, true);
    setOptionState(RESP_MECH, true);
    setOptionState(TRENDING, true);
}
/*
 * Packs an 2N-character, human-readable vent serial number into a N/2-byte binary hex sequence.
 * Note that this class only uses N=5.
 * For e.g. "1234567890" will become 0x1234567890
 * This is done by mostly converting the ASCII character to its corresponding binary number. For
 * e.g. a '1' becomes number 1, which can be represented by a nibble (4 bits). The characters in
 * the vent serial numbers are limited - just 13 valid characters. So it is possible to represent
 * every valid character with 4 bits.
 *
 * Valid characters are characters '0'-'9' (mapped to 0x0-0x9), 'G' (mapped to '0xA'), 
 * 'B' (mapped to '0xB'), 'C' (mapped to '0xC') and 'P' (mapped to '0xD'). Both upper case and
 * lower case versions are accepted for valid alphabets.
 *
 * NOTE: the input dstBuffer *MUST* be pre-allocated (with a minimum of half the length of input
 * string str.
 */
bool SoftwareOptionsKey::_convertToPackedBCH(const std::string& str, Uint8* dstBuffer) const
{
    bool ret = true;
    Uint8 tempChar;
    size_t pos = 0;
    while(pos != str.length())
    {
        char srcChar = str.at(pos);
        switch (srcChar)
        {
            case ('0'):
            case ('1'):
            case ('2'):
            case ('3'):
            case ('4'):
            case ('5'):
            case ('6'):
            case ('7'):
            case ('8'):
            case ('9'):
            tempChar = (Uint8) (((Uint8)srcChar) - '0');
            break;

            case ('G'):
            case ('g'):
            // Transform 'G' for "GUI" to hex 0x0a.
            tempChar = 10;
            break;

            case ('B'):
            case ('b'):
            // Transform for "Breath-Delivery" to hex 0x0b.
            tempChar = 11;
            break;

            case ('C'):
            case ('c'):
            // Transform for "Compressor" to hex 0x0c.
            tempChar = 12;
            break;

            case ('P'):
            case ('p'):
            // Transform for "Prototype" to hex 0x0d.
            tempChar = 13;
            break;

            default:
            tempChar = 15; // 15 == 'F' (hex) FAILURE!
            ret = false;
            break;
        }
        if(ret)
        {
            int byteIndex = pos/2;
            if(pos%2)
            {
                dstBuffer[byteIndex] |= tempChar;
            }
            else
            {
                dstBuffer[byteIndex] |= (tempChar << 4);
            }
        }
        else
        {
            break;
        }
        pos++;
    }
    return ret;
}
/* See the note about logic for 'packing'. This API just undoes that logic - i.e. an N-byte packed hexa decimal 
 * sequence is converted to 2N-character, human-readable string. The use of this API in this class is limited to
 * just N=5.
 */
void SoftwareOptionsKey::_unpackBCHString(const Uint8* packedBuffer, Uint32 len, std::string& dstString) const
{
    bool failed = false;
    for(Uint32 i=0; i<len; i++)
    {
        FancyByte thisByte(packedBuffer[i]);

        if(thisByte.msnibble() < 10)
            dstString.push_back(char(thisByte.msnibble() + '0'));
        else
        {
            switch(thisByte.msnibble())
            {
            case 10: dstString.push_back('G'); break; 
            case 11: dstString.push_back('B'); break; 
            case 12: dstString.push_back('C'); break; 
            case 13: dstString.push_back('P'); break; 
            default: failed = true;
            };
        }
        if(thisByte.lsnibble() < 10)
            dstString.push_back(char(thisByte.lsnibble() + '0'));
        else
        {
            switch(thisByte.lsnibble())
            {
            case 10: dstString.push_back('G'); break; 
            case 11: dstString.push_back('B'); break; 
            case 12: dstString.push_back('C'); break; 
            case 13: dstString.push_back('P'); break; 
            default: failed = true;
            }
        }
    }
    if(failed)
        dstString = "FAILED";

}


Uint32 SoftwareOptionsKey::_getValidationCode(const Uint8* bchCodedVsn, Uint32 bchLen, 
        const SoftwareOptionsKey::ConfigurationCode_& code) const
{

    //Handle the different endian-ness between the PC-side (where key generation happens) 
    //and the vent-side (where the key is used)
    SoftwareOptionsKey::ConfigurationCode_ tempCode = code;
    tempCode.customerOptions = 
        (((code.customerOptions & 0x000000ff) << 24) |
        ((code.customerOptions & 0x0000ff00) << 8)  |
        ((code.customerOptions & 0x00ff0000) >> 8)  |
        ((code.customerOptions & 0xff000000) >> 24));
    const Uint8 *pByte = (const Uint8 *)&tempCode ;

    /* 
     * There are many possible ways of generating the validation code, I guess. But
     * this algorithm was apparently taken as is from PB840. It just creates a hash
     * of bytes from BCH encoded vent serial number and the configuration code (that
     * includes the customer options)
     */
    Uint32 weightFactor[] = {1, 2, 3, 5, 7, 11, 13, 17, 19, 23} ;
    Uint32 configChecksum = 0 ;
    for (Uint32 ii=0; ii < sizeof(SoftwareOptionsKey::ConfigurationCode_); ii++)
    {
        configChecksum += *(pByte + ii) * *(pByte + ii) * weightFactor[ii] ;
    }

    Uint32 snChecksum = 0 ;
    for (Uint32 ii=0; ii < bchLen; ii++)
    {
        snChecksum += *(bchCodedVsn + ii) * *(bchCodedVsn + ii) * weightFactor[ii] ;
    }

    Uint32 total = snChecksum + configChecksum ;

    const Uint32 MAX_FACTOR = 412 ;

    Uint32 factor = (total % MAX_FACTOR) + 1 ;

    Uint32 validationCode = total * factor ;

    return validationCode;
}

void SoftwareOptionsKey::_encrypt(const Uint8* unencryptedBuffer, Uint32 len, std::string& encryptedString) const
{
    //Step 0: Convert the unencryptedBuffer (a byte array) into a nibble array, with each nibble stored as its own byte
    std::vector<Uint8> nibbleArray(len*2);
    for(Uint32 i=0; i<len; i++)
    {
        nibbleArray[2*i] = unencryptedBuffer[i] >> 4;
        nibbleArray[2*i+1] = unencryptedBuffer[i] & 0xF;
    }


    //Step 1: Swap N[k] <-> N[K+1]
    //NOTE/TODO: This is probably more efficient to combine this with Step 0, so we dont have to do an
    //explicit swap. But I am just sticking with "original" implementation for now - Base(7/24/2013)
    for(Uint32 i=0; i<len; i++)
    {
        Uint8 temp = nibbleArray[2*i];
        nibbleArray[2*i] = nibbleArray[2*i+1];
        nibbleArray[2*i+1] = temp;
    }

    //Step 2: N[k] = 15-N[k]
    //NOTE/TODO: Again, probably more efficient to combine with earlier step
    for(Uint32 i=0; i<2*len; i++)
    {
        nibbleArray[i] = 15-nibbleArray[i];
    }

    //Step 3: N[k] = (N[k] + {1, 3, 5, 7, 9, 11, 13, 15}) % 16
	const Uint8 WEIGHING_FACTORS[] = {1, 3, 5, 7, 9, 11, 13, 15};
    for(Uint32 i=0; i<2*len; i++)
    {
        nibbleArray[i] = (nibbleArray[i] + WEIGHING_FACTORS[i%sizeof(WEIGHING_FACTORS)]) % 16;
    }

    //Now, combine the nibbles back to bytes
    std::vector<Uint8> encryptedBuffer(len);
    for(Uint32 i=0; i<len; i++)
    {
        encryptedBuffer[i] = (nibbleArray[2*i] << 4) | nibbleArray[2*i+1];
    }

    //Format to human readable text
    //Formatting Step 1: Take two most significant bits from every 3-byte chunks, append 
    //them together to create a new byte, and create a 4-byte chunk starting with the
    //newly created byte. At the end of this step, N-byte buffer will become 4N/3 byte buffer.
    std::vector<Uint8> formattedBuffer(4*len/3);
    for(Uint32 i=0,j=0; i<len; i+=3, j+=4)
    {
        formattedBuffer[j] = ((encryptedBuffer[i] & 0xC0) >> 2) | 
                                ((encryptedBuffer[i+1] & 0xC0)>>4) | 
                                ((encryptedBuffer[i+2] & 0xC0) >> 6);
        formattedBuffer[j+1] = encryptedBuffer[i] & 0x3F;
        formattedBuffer[j+2] = encryptedBuffer[i+1] & 0x3F;
        formattedBuffer[j+3] = encryptedBuffer[i+2] & 0x3F;
    }

    //Formatting Step 2: Table-based translation of bytes to create human readable characters
	static const unsigned char BIN_TO_HUMAN[] =
	{
	0x40, /* 0x00 to '@'*/ 0x41, /* 0x01 to 'A'*/ 0x42, /* 0x02 to 'B'*/ 0x43, // 0x03 to 'C'
	0x44, /* 0x04 to 'D'*/ 0x45, /* 0x05 to 'E'*/ 0x46, /* 0x06 to 'F'*/ 0x47, // 0x07 to 'G'
	0x48, /* 0x08 to 'H'*/ 0x23, /* 0x09 to '#' EXCLUDE 'I'*/ 0x4A, // 0x0A to 'J'
	0x4B, /* 0x0B to 'K'*/ 0x24, /* 0x0C to '$' EXCLUDE 'L'*/ 0x4D, // 0x0D to 'M'
	0x4E, /* 0x0E to 'N'*/ 0x32, /* 0x0F to '2' EXCLUDE 'O'*/ 0x50, // 0x10 to 'P'
	0x51, /* 0x11 to 'Q'*/ 0x52, /* 0x12 to 'R'*/ 0x53, /* 0x13 to 'S'*/ 0x54, // 0x14 to 'T'
	0x55, /* 0x15 to 'U'*/ 0x56, /* 0x16 to 'V'*/ 0x57, /* 0x17 to 'W'*/ 0x58, // 0x18 to 'X'
	0x59, /* 0x19 to 'Y'*/ 0x5A, /* 0x1A to 'Z'*/ 0x2A, // 0x1B to '*' EXCLUDE '['
	0x33, /* 0x1C to '3' EXCLUDE '\'*/ 0x2B, /* 0x1D to '+' EXCLUDE ']'*/ 0x5E, // 0x1E to '^'
	0x34, /* 0x1F to '4' EXCLUDE '_'*/ 0x35, /* 0x20 to '5' EXCLUDE '''*/ 0x61, // 0x21 to 'a'
	0x62, /* 0x22 to 'b'*/ 0x63, /* 0x23 to 'c'*/ 0x64, /* 0x24 to 'd'*/ 0x65, // 0x25 to 'e'
	0x66, /* 0x26 to 'f'*/ 0x67, /* 0x27 to 'g'*/ 0x68, /* 0x28 to 'h'*/ 0x36, // 0x29 to '6' EXCLUDE 'i'
	0x6A, /* 0x2A to 'j'*/ 0x6B, /* 0x2B to 'k'*/ 0x37, /* 0x2C to '7' EXCLUDE 'l'*/ 0x6D, // 0x2D to 'm'
	0x6E, /* 0x2E to 'n'*/ 0x38, /* 0x2F to '8' EXCLUDE 'o'*/ 0x70, /* 0x30 to 'p'*/ 0x71, // 0x31 to 'q'
	0x72, /* 0x32 to 'r'*/ 0x73, /* 0x33 to 's'*/ 0x74, /* 0x34 to 't'*/ 0x75, // 0x35 to 'u'
	0x76, /* 0x36 to 'v'*/ 0x77, /* 0x37 to 'w'*/ 0x78, /* 0x38 to 'x'*/ 0x79, // 0x39 to 'y'
	0x7A, /* 0x3A to 'z'*/ 0x25, /* 0x3B to '%' EXCLUDE '{'*/ 0x21, // 0x3C to '!' EXCLUDE '|'
	0x26, /* 0x3D to '&' EXCLUDE '}'*/ 0x7E, /* 0x3E to '~'*/ 0x39  // 0x3F to '9' EXCLUDE []
	};

	for (Uint32 i=0; i < formattedBuffer.size(); i++)
	{
		formattedBuffer[i] = BIN_TO_HUMAN[formattedBuffer[i]];
	}

    //Append a NULL character before copying to output string
    formattedBuffer.push_back(0);
    encryptedString = (char*)(&formattedBuffer[0]);
}

bool SoftwareOptionsKey::_decrypt(const std::string& encryptedKey, PlainSoftwareKey& key) const
{
    //First some basic error checking
    //The encryption algorithm takes the PlainSoftwareKey and creates 4 human-readable characters
    //for every byte (i.e. resulting human-readable key has sizeof(PlainSoftwareKey)*4/3
    //characters in it. If that relation is not valid with input key, we cant decrypt it here
    if(encryptedKey.length() != ENCRYPTED_BYTE_STREAM_SIZE)
    {
        std::cout << "Invalid KEY!! " << encryptedKey.length() << " AND " << sizeof(key) << std::endl;
        return false;
    }
    //"Unformat" Step 1: Do table-based reverse converstion from human-readable characters to
    //plain binary
    const unsigned char HUMAN_TO_BIN[] = 
    {
    0xFF, /* 0x00 */ 0xFF, /* 0x01 */ 0xFF, /* 0x02 */ 0xFF, /* 0x03 */ 0xFF, // 0x04
    0xFF, /* 0x05 */ 0xFF, /* 0x06 */ 0xFF, /* 0x07 */ 0xFF, /* 0x08 */ 0xFF, // 0x09
    0xFF, /* 0x0A */ 0xFF, /* 0x0B */ 0xFF, /* 0x0C */ 0xFF, /* 0x0D */ 0xFF, // 0x0E
    0xFF, /* 0x0F */ 0xFF, /* 0x10 */ 0xFF, /* 0x11 */ 0xFF, /* 0x12 */ 0xFF, // 0x13
    0xFF, /* 0x14 */ 0xFF, /* 0x15 */ 0xFF, /* 0x16 */ 0xFF, /* 0x17 */ 0xFF, // 0x18
    0xFF, /* 0x19 */ 0xFF, /* 0x1A */ 0xFF, /* 0x1B */ 0xFF, /* 0x1C */ 0xFF, // 0x1D
    0xFF, /* 0x1E */ 0xFF, /* 0x1F */ 0xFF, /* 0x20 */ 0x3C, /* 0x21 */ 0xFF, // 0x22
    0x09, /* 0x23 */ 0x0C, /* 0x24 */ 0x3B, /* 0x25 */ 0x3D, /* 0x26 */ 0xFF, // 0x27
    0xFF, /* 0x28 */ 0xFF, /* 0x29 */ 0x1B, /* 0x2A */ 0x1D, /* 0x2B */ 0xFF, // 0x2C
    0xFF, /* 0x2D */ 0xFF, /* 0x2E */ 0xFF, /* 0x2F */ 0xFF, /* 0x30 '0' EXCLUDED */ 0xFF, // 0x31 '1' EXCLUDED
    0x0F, /* 0x32 '2' */ 0x1C, /* 0x33 '3' */ 0x1F, /* 0x34 '4' */ 0x20, /* 0x35 '5' */ 0x29, // 0x36 '6'
    0x2C, /* 0x37 '7' */ 0x2F, /* 0x38 '8' */ 0x3F, /* 0x39 '9' */ 0xFF, /* 0x3A */ 0xFF, // 0x3B
    0xFF, /* 0x3C */ 0xFF, /* 0x3D */ 0xFF, /* 0x3E */ 0xFF, /* 0x3F */ 0x00, // 0x40 '@'
    0x01, /* 0x41 'A' */ 0x02, /* 0x42 'B' */ 0x03, /* 0x43 'C' */ 0x04, /* 0x44 'D' */ 0x05, // 0x45 'E'
    0x06, /* 0x46 'F' */ 0x07, /* 0x47 'G' */ 0x08, /* 0x48 'H' */ 0xFF, /* 0x49 'I' EXCLUDED */ 0x0A, // 0x4A 'J'
    0x0B, /* 0x4B 'K' */ 0xFF, /* 0x4C 'L' EXCLUDED */ 0x0D, /* 0x4D 'M' */ 0x0E, // 0x4E 'N'
    0xFF, /* 0x4F 'O' EXCLUDED */ 0x10, /* 0x50 'P' */ 0x11, /* 0x51 'Q' */ 0x12, // 0x52 'R'
    0x13, /* 0x53 'S' */ 0x14, /* 0x54 'T' */ 0x15, /* 0x55 'U' */ 0x16, /* 0x56 'V' */ 0x17, // 0x57 'W'
    0x18, /* 0x58 'X' */ 0x19, /* 0x59 'Y' */ 0x1A, /* 0x5A 'Z' */ 0xFF, // 0x5B '[' EXCLUDED
    0xFF, /* 0x5C '\' EXCLUDED */ 0xFF, /* 0x5D ']' EXCLUDED */ 0x1E, /* 0x5E '^' */ 0xFF, // 0x5F '_' EXCLUDED
    0xFF, /* 0x60 '`' EXCLUDED */ 0x21, /* 0x61 'a' */ 0x22, /* 0x62 'b' */ 0x23, // 0x63 'c'
    0x24, /* 0x64 'd' */ 0x25, /* 0x65 'e' */ 0x26, /* 0x66 'f' */ 0x27, /* 0x67 'g' */ 0x28, // 0x68 'h'
    0xFF, /* 0x69 'i' EXCLUDED */ 0x2A, /* 0x6A 'j' */ 0x2B, /* 0x6B 'k' */ 0xFF, // 0x6C 'l' EXCLUDED
    0x2D, /* 0x6D 'm' */ 0x2E, /* 0x6E 'n' */ 0xFF, /* 0x6F 'o' EXCLUDED */ 0x30, /* 0x70 'p' */ 0x31, // 0x71 'q'
    0x32, /* 0x72 'r' */ 0x33, /* 0x73 's' */ 0x34, /* 0x74 't' */ 0x35, /* 0x75 'u' */ 0x36, // 0x76 'v'
    0x37, /* 0x77 'w' */ 0x38, /* 0x78 'x' */ 0x39, /* 0x79 'y' */ 0x3A, /* 0x7A 'z' */ 0xFF, // 0x7B '{' EXCLUDED
    0xFF, /* 0x7C '|' EXCLUDED */ 0xFF, /* 0x7D '}' EXCLUDED */ 0x3E, /* 0x7E '~' */ 0xFF // 0x7F [] EXCLUDED
    };

    std::vector<Uint8> encryptedBuffer(encryptedKey.length());
    for(Uint32 i=0; i < encryptedKey.length(); i++)
    {
        Uint8 temp = encryptedKey.at(i);
        if(temp < 0x7F)
            encryptedBuffer[i] = HUMAN_TO_BIN[temp];
        else
            //It cant be human readable. return.
            return false;
    }

    //"Unformatting" Step 2: Pack the the 4-byte chunks to 3-byte chunks. See Encyrption API for more
    //details. The "unpacking" during encryption makes the size 4/3 times.. so the decryption will 
    //make the size back .. i.e.  3/4 of encrypted size.
    Uint32 decryptedLen = encryptedKey.length()*3/4;
    std::vector<Uint8> decryptedBuffer(decryptedLen);
    for(Uint32 i=0, j=0; i < encryptedKey.length(); i+=4,j+=3)
    {
        decryptedBuffer[j] = ((encryptedBuffer[i] & 0x30) << 2) | encryptedBuffer[i+1];
        decryptedBuffer[j+1] = ((encryptedBuffer[i] & 0xC) << 4) | encryptedBuffer[i+2];
        decryptedBuffer[j+2] = ((encryptedBuffer[i] & 0x3) << 6) | encryptedBuffer[i+3];
    }

    //Decryption Step 0: Split the bytes into nibbles, and put each one in a separate byte
    std::vector<Uint8> nibbleArray(decryptedLen*2);
    for(Uint32 i=0; i<decryptedLen; i++)
    {
        nibbleArray[2*i] = decryptedBuffer[i] >> 4;
        nibbleArray[2*i+1] = decryptedBuffer[i] & 0xF;
    }

    //Decryption Step 1: N[k]=N[k]-{1,3,5,7,9,11,13,15} ?>=0 : +16.
	const Uint8 WEIGHING_FACTORS[] = {1, 3, 5, 7, 9, 11, 13, 15};
    for(Uint32 i=0; i<2*decryptedLen; i++)
    {
        nibbleArray[i] = (nibbleArray[i] - WEIGHING_FACTORS[i%sizeof(WEIGHING_FACTORS)]) % 16;
        if((int8)(nibbleArray[i]) < 0)
            nibbleArray[i] += 16;
    }

    //Decryption Step 2: N[k] = 15-N[k]
    for(Uint32 i=0; i<2*decryptedLen; i++)
    {
        nibbleArray[i] = 15-nibbleArray[i];
    }

    //Decryption Step 3: Swap N[k] <-> N[K=1]
    for(Uint32 i=0; i<decryptedLen; i++)
    {
        Uint8 temp = nibbleArray[2*i];
        nibbleArray[2*i] = nibbleArray[2*i+1];
        nibbleArray[2*i+1] = temp;
    }

    //Now, combine the nibbles back to bytes
    for(Uint32 i=0; i<decryptedLen; i++)
    {
        decryptedBuffer[i] = (nibbleArray[2*i] << 4) | nibbleArray[2*i+1];
    }

    //Decryption complete! Copy the decrypted byte stream into the key.
    key.setFromByteStream(&decryptedBuffer[0], decryptedLen);
    return false;
}
bool SoftwareOptionsKey::_decodeToKey(const std::string& encryptedKey, const std::string& ventSN, PlainSoftwareKey& key)
{
    bool ret = false;
    //Decrypt the encrypted key and initialize the internal key fields
    _decrypt(encryptedKey, key);
    //Do some checking whether key is valid.
    //First, compute the validation code and see if it matches what came with the key. This just verifies
    //that the key wasnt corrupt, but doesnt verify that it belongs to "this" (i.e. ventVSN) vent.
    Uint32 code = _getValidationCode(key.ventSerialNumber, sizeof(key.ventSerialNumber), 
                                                                    key.configurationCode);
    if(code == key.validationCode)
    {
        //Now if the key is really meant for this vent (i.e. the vent identified by ventVSN)
        std::string keyVSN;
        _unpackBCHString(key.ventSerialNumber, PACKED_VENT_SERIAL_NUMBER_LEN, keyVSN);
        if((keyVSN.length() == ventSN.length()) &&
                (strncasecmp(keyVSN.c_str(), ventSN.c_str(), keyVSN.length()) == 0))
        {
            //The key is valid, and is meant for this vent
            ret = true;
        }
        else
        {
            //Ok, so the key wasnt specific to this vent. But it could still be usable if it is
            //SALES or MANUFACTURING type. Check that.
            //For sales and manufacturing, we just leave the key alone. This allows keys
            //that work on any vent. We are not enabling all options still, so they have to
            //create separate keys for every options combinations they need enabled - or just 
            //one that has all options enabled.
            if((key.configurationCode.keyType != MANUFACTURING) &&
                    (key.configurationCode.keyType != SALES))
            {
                //The key is not usable. Revert the internal variables to its default state
                //This means the always-on options will be reported as enabled, and everything
                //else would be disabled.
                std::cout << "Key cannot be used on this vent..." << std::endl;
                std::cout << "Created for "  << keyVSN << ", and this is " << ventSN << std::endl;
                std::cout << key.getAsString() << std::endl;
            }
            else
            {
                //The key type is SALES or MANUFACTURING.. can be used even if the serial numbers
                //didnt match.
                return true;
            }
        }
    }
    else
    {
        //The validation code in the key, and the validation code that should have been on the
        //key are not the same. In other words, the key was corrupt. Throw it away
        std::cout << std::hex << "Invalid validation code... Expected " << code 
            << ", Got " << key.validationCode << std::dec << std::endl;
        std::cout << key.getAsString() << std::endl;
    }
    return ret;
}


