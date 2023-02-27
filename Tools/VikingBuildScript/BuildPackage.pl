#----------------------------------------------------------------------------
#            Copyright (c) 2012 Covidien, Inc.
#
# This software is copyrighted by and is the sole property of Covidien. This
# is a proprietary work to which Covidien claims exclusive right.  No part
# of this work may be used, disclosed, reproduced, stored in an information
# retrieval system, or transmitted by any means, electronic, mechanical,
# photocopying, recording, or otherwise without the prior written permission
# of Covidien.
#----------------------------------------------------------------------------
use strict;
use Digest::MD5;
use File::Find;
use MIME::Base64;
use Cwd;
use IO::Compress::Gzip qw(gzip $GzipError) ;

#------------------------------------------------------------------------------
# md5sum
#------------------------------------------------------------------------------
sub md5sum
{
	my $file = shift;
	
    open(FILE, $file) or die "Can't open '$file': $!";
	binmode(FILE);
	
    my $md5 = Digest::MD5->new;
	
    while (<FILE>) 
    {
		$md5->add($_);
	}

	close(FILE);

	return $md5->hexdigest;
}

#------------------------------------------------------------------------------
# filelength
#------------------------------------------------------------------------------
sub filelength
{
	return (-s shift)+1024;
}

#------------------------------------------------------------------------------
# base64AFile
#------------------------------------------------------------------------------
sub base64AFile
{
    my $file = shift;
    my $stream =shift;
    my $buf;

    open(FILE, $file) or die "$!";

    while (read(FILE, $buf, 60*57))
    {
        print $stream encode_base64($buf);
    }
    close(FILE);
}

#------------------------------------------------------------------------------
# outputFile
#------------------------------------------------------------------------------
sub outputFile
{
    my $file = shift;
    my $stream =shift;
    my $cwd =shift;

    my $newfile = $cwd."/".$file;

    my $md5 = md5sum($newfile);
    my $length=filelength($newfile);


    $file =~ s/[\/]/\\/g; 


    printf $stream "<file name='\\$file' md5sum='$md5' length='$length'>\n";

    base64AFile($newfile,$stream);

    printf $stream "</file>\n";
}

#------------------------------------------------------------------------------
# makeDownloadPackage
#------------------------------------------------------------------------------
sub makeDownloadPackage
{
    my $dir = shift || die "Argument missing: directory name\n";
    my $stream = shift || die "Argument missing: stream\n";

    chdir $dir or die "Cant chdir to $dir $!";

    my $cwd = getcwd();


    my $z = new IO::Compress::Gzip $stream ,(-Level=>9) or die "gzip failed: $GzipError\n";


    printf $z "<files>\n";
    find( 
        sub 
        {
            if (/\.(xml|gz|bin|rbf)$/)
            {
                my $fileName = $File::Find::name;

                $fileName =~ s/^[.][\/]//g; 

               
                outputFile($fileName,$z,$cwd);
            }
        }
        ,"."
    );
    printf $z "</files>\n";

    $z->close() ;
}

#------------------------------------------------------------------------------
# main
#------------------------------------------------------------------------------
sub main
{
    makeDownloadPackage(shift, *STDOUT);
}

main(shift);
