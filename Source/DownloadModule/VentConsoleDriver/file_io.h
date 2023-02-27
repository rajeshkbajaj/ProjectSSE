#ifndef IO_H
#define IO_H
//----------------------------------------------------------------------------
//            Copyright (c) 2011 Covidien, Inc.
//
// This software is copyrighted by and is the sole property of Covidien. This
// is a proprietary work to which Covidien claims exclusive right.  No part
// of this work may be used, disclosed, reproduced, stored in an information
// retrieval system, or transmitted by any means, electronic, mechanical,
// photocopying, recording, or otherwise without the prior written permission
// of Covidien.
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
/// @file file_io.h
/// @note All data types are native to gcc for comparability with libc.
//----------------------------------------------------------------------------


// Error types
#define COV_WRONG_NUMBER_ARGS  -1
#define COV_ARG_TYPE_MISMATCH  -2
#define COV_DATA_SIZE_MISMATCH -3

// Call types
#define FREAD       1
#define FWRITE      2
#define FCLOSE      3
#define COV_FOPEN       4
#define FSEEK       5
#define FPRINTF     6
#define FTELL       7
#define SETBUF      8
#define FILENO      9
#define FTRUNCATE   10
#define OPEN        14
#define CLOSE       15
#define READ        16
#define WRITE       17
#define LINK        18
#define UNLINK      19
#define LSEEK       20

// Serialization types
#define TYPEID_BYTE        1
#define TYPEID_INT         2
#define TYPEID_RAWBYTE     3
#define TYPEID_CHARSZ      4
#define TYPEID_SIZET       5
#define TYPEID_MODET       6
#define TYPEID_OFFT        7
#define TYPEID_NIL         0
#define TYPEID_FILET       8



typedef struct{
    unsigned int session_id;
    unsigned int session_state;
    unsigned int session_sequence;
    unsigned int length;
} Message;


typedef struct{
	unsigned char remote_fileio_head[16];
    unsigned int type;
    unsigned int length;
} CallMessage;


typedef struct {
    unsigned int type;
    unsigned int length;

#pragma warning(push)
#pragma warning(disable : 4200)
    char value[0];
#pragma warning(pop)
} ByteMessage;

typedef struct {
    unsigned int type;
    unsigned int value;
} IntMessage;


#endif
