/*++

Module Name:

    ztex.h

Abstract:

    Definitions for compressed ZenGin Textures (.tex)

Author:

    Nico Bendlin <nicode@gmx.net>

Revision History:

    2005-04-25  Final/reviewed release (1.0)
    2005-04-04  First public release (draft)

--*/

#ifndef ZTEX_H_INCLUDE_GUARD
#define ZTEX_H_INCLUDE_GUARD

/* ZenGin Texture - Render Formats */
typedef enum _ZTEX_FORMAT {
    ZTEXFMT_B8G8R8A8,  /* 0, 32-bit ARGB pixel format with alpha, using 8 bits per channel */
    ZTEXFMT_R8G8B8A8,  /* 1, 32-bit ARGB pixel format with alpha, using 8 bits per channel */
    ZTEXFMT_A8B8G8R8,  /* 2, 32-bit ARGB pixel format with alpha, using 8 bits per channel */
    ZTEXFMT_A8R8G8B8,  /* 3, 32-bit ARGB pixel format with alpha, using 8 bits per channel */
    ZTEXFMT_B8G8R8,    /* 4, 24-bit RGB pixel format with 8 bits per channel */
    ZTEXFMT_R8G8B8,    /* 5, 24-bit RGB pixel format with 8 bits per channel */
    ZTEXFMT_A4R4G4B4,  /* 6, 16-bit ARGB pixel format with 4 bits for each channel */
    ZTEXFMT_A1R5G5B5,  /* 7, 16-bit pixel format where 5 bits are reserved for each color and 1 bit is reserved for alpha */
    ZTEXFMT_R5G6B5,    /* 8, 16-bit RGB pixel format with 5 bits for red, 6 bits for green, and 5 bits for blue */
    ZTEXFMT_P8,        /* 9, 8-bit color indexed */
    ZTEXFMT_DXT1,      /* A, DXT1 compression texture format */
    ZTEXFMT_DXT2,      /* B, DXT2 compression texture format */
    ZTEXFMT_DXT3,      /* C, DXT3 compression texture format */
    ZTEXFMT_DXT4,      /* D, DXT4 compression texture format */
    ZTEXFMT_DXT5,      /* E, DXT5 compression texture format */
    ZTEXFMT_COUNT
} ZTEX_FORMAT;

/* ZenGin Texture - Info Block */
typedef struct _ZTEX_INFO {
    unsigned long   Format;     /* ZTEXFMT_ */
    unsigned long   Width;      /* mipmap 0 */
    unsigned long   Height;     /* mipmap 0 */
    unsigned long   MipMaps;    /* 1 = none */ 
    unsigned long   RefWidth;   /* ingame x */
    unsigned long   RefHeight;  /* ingame y */
    unsigned long   AvgColor;   /* A8R8G8B8 */
} ZTEX_INFO;

/* ZenGin Texture - File Signature */
#if !defined(__BIG_ENDIAN__) && !defined(_MAC)
#define ZTEX_FILE_SIGNATURE 0x5845545A  /* 'XETZ' (little-endian) */
#else
#define ZTEX_FILE_SIGNATURE 0x5A544558  /* 'ZTEX' (big-endian) */
#endif

/* ZenGin Texture - File Version */
#define ZTEX_FILE_VERSION_0 0x00000000

/* ZenGin Texture - File Header */
typedef struct _ZTEX_FILE_HEADER {
    unsigned long   Signature;  /* ZTEX_FILE_SIGNATURE */
    unsigned long   Version;    /* ZTEX_FILE_VERSION_0 */
    ZTEX_INFO       TexInfo;
} ZTEX_FILE_HEADER;

/* ZenGin Texture - Palette Entry */
typedef struct _ZTEX_PAL_ENTRY {
    unsigned char   r;
    unsigned char   g;
    unsigned char   b;
} ZTEX_PAL_ENTRY;

/* ZenGin Texture - Number of Palette Entries */
#define ZTEX_PAL_ENTRIES 0x0100

/* ZenGin Texture - Stored Palette */
typedef struct _ZTEX_PALETTE {
    ZTEX_PAL_ENTRY  Entries[ZTEX_PAL_ENTRIES];
} ZTEX_PALETTE;

#endif /* ZTEX_H_INCLUDE_GUARD */
