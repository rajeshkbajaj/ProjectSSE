#ifndef __Software_Options_Key_H__
#define __Software_Options_Key_H__
#include <string>
///TODO: Remove these typedefs in favor of Sigma.h when merging to main line
#if defined WIN32
typedef unsigned int Uint32;
typedef int int32;
typedef unsigned char Uint8;
typedef char int8;

//Simple abstraction for timestamp.. 
#include <windows.h>
class TimeStamp
{
public:
    TimeStamp()
    {
        GetLocalTime(&_time);
    }
    Uint32 getYear() { return _time.wYear; }
    Uint32 getMonth() { return _time.wMonth; }
    Uint32 getDayOfMonth() { return _time.wDay; }
private:
    SYSTEMTIME _time;
};

#endif

class SoftwareOptionsKey
{
public:
	enum KeyType: Uint8
    {
        RAWINVALID = 0,
        NORMAL,
        ENGINEERING,
        SALES,
        MANUFACTURING,
        LAST_VALID_KEY_TYPE = MANUFACTURING,
        NUM_DATAKEYTYPES
    };
  	enum OptionId
	{
        RESERVED1 = 0,
        ATC,
        NEO_MODE,
        VTPC,
        RESP_MECH,
        TRENDING,
        NEO_MODE_UPDATE,
        LAST_840_OPTION = NEO_MODE_UPDATE,
        FIRST_VIKING_OPTION,
        NEO_MODE_ADVANCED = FIRST_VIKING_OPTION,
        NEO_MODE_LOCKOUT,
        BILEVEL,
        PAV,
        IE_SYNC_ASYNC,
        LEAK_COMP,
        ESM,
		CAPNOGRAPHY,
        NUM_OPTIONS
	};

    SoftwareOptionsKey();
	/**
	@detail Initialize the instance with specified key, and verify it against the
	specified vent serial number. If the key was created for the specified vent
	serial number, the instance is initialized, and the API returns true. If the
	key wasnt created for the specified vent serial number, the instance stays 
	uninitialized, and the API returns false.
	*/
    bool updateKey(const std::string& encryptedKey, const std::string& ventSN);

	/**
	@detail Get the state (Enabled/Disabled) of the specified option. For options
    that are time-limited, the API also verifies the date, so there is no need for
    callers to check against the current date again.
	*/
    bool getOptionState(OptionId id) const;
    /**
     * @detail Returns the vent serial number of this key
     */
    const std::string getVentSerialNumber() const;
    /**
     * @detail Returns the current key type
     */
    KeyType getKeyType() const;
    /**
     * @detail Returns a string that explains the current key. It includes the current
     * state of all options. It is mainly intended for debugging.
     */
    const std::string getExplanatoryString() const;
    /**
     * @detail Returns the 24-character encrypted key string.
     */
    const std::string getEncryptedKey() const;

    //We may want to enable these APIs only for Key Creator through a conditional
    //compilation flag. For now, just leave it on everywhere
    /**
     * @detail Enables/disables an option
     */
    void setOptionState(OptionId id, bool state);
    /** 
     * @detail Sets the vent serial number for which this key should apply
     */
    bool setVentSerialNumber(const std::string& vsn);
    /**
     * @detail Sets the type of the key
     */
    void setKeyType(KeyType type);
    /**
     * @detail Sets the expiry date for an option. If the option is not time-limited,
     * or if the date is wrong the API returns false.
     */
    bool setKeyExpiryDate(OptionId id, Uint8 day, Uint8 month, Uint8 year);
    /**
     * @details returns the expiry date corresponding to this option. If the option is
     * not time-limited the values on day/month/year would be 0.
     */
    void getKeyExpiryDate(OptionId id, Uint8& day, Uint8& month, Uint8& year);

    static SoftwareOptionsKey* getInstance();
private:
    static const Uint32 UNENCRYPTED_BYTE_STREAM_SIZE = 18;
    static const Uint32 ENCRYPTED_BYTE_STREAM_SIZE = 24;
    struct ConfigurationCode_
    {
        Uint32 customerOptions ;
        KeyType keyType;
        Uint8 expirationMonth;
        Uint8 expirationDay;
        Uint8 expirationYear;
        ConfigurationCode_();

        const std::string getAsString() const;
        //These APIs deal with un-padded memory layout of the
        //struct, unlike a plain cast to Uint8*
        bool getToByteStream(Uint8* buffer, Uint32 len) const;
        bool setFromByteStream(const Uint8* buffer, Uint32 len);
        Uint32 getUnpaddedSize() const;
    };

    /**
     * @class PlainSoftwareKey
     * @detail In-memory layout of the Software Option Key in plain form. 
     */
    struct PlainSoftwareKey
    {
        Uint32 validationCode;
        Uint8 ventSerialNumber[5];
        ConfigurationCode_ configurationCode;
        Uint8 programmerMode;
        PlainSoftwareKey();
        const std::string getAsString() const;
        //These APIs deal with un-padded memory layout of the
        //struct, unlike a plain cast to Uint8*
        bool getToByteStream(Uint8* buffer, Uint32 len) const;
        bool setFromByteStream(const Uint8* buffer, Uint32 len);
        Uint32 getUnpaddedSize() const;
        bool merge(const PlainSoftwareKey& frmKey);

    };
    union FancyByte
    {
        Uint8 allbyte;
        Uint8 lsnibble() { return allbyte & 0xf; }
        Uint8 msnibble() { return ((allbyte & 0xf0) >> 4); }
        FancyByte(Uint8 b)
            :allbyte(b)
        {
        }
    };

    PlainSoftwareKey _key;
    bool _initialized;

    static SoftwareOptionsKey* _instance;

    void _init();
    //Some of these APIs may be moved to the ConfigurationCode_ and/or PlainSoftwareKey
    //structs later.
    bool _convertToPackedBCH(const std::string& str, Uint8* dstBuffer) const;
    void _unpackBCHString(const Uint8* packedBuffer, Uint32 len,  std::string& dstString) const;
    Uint32 _getValidationCode(const Uint8* bchCodedVsn, Uint32 bchLen, const ConfigurationCode_& code) const;
    void _encrypt(const Uint8* unencryptedBuffer, Uint32 len, std::string& encryptedString) const;
    bool _decrypt(const std::string& encryptedKey, PlainSoftwareKey& key) const;
    bool _decodeToKey(const std::string& encryptedKey, const std::string& ventSN, PlainSoftwareKey& key);
};

#endif

