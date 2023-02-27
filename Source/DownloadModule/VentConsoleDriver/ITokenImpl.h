#ifndef ITOKENIMPL_H
#define ITOKENIMPL_H
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


class ITokenImpl
{
    public:
        virtual const char *getValue(unsigned int r) = 0;
        virtual const int getLength() = 0;
        virtual ~ITokenImpl(){}
};

#endif
