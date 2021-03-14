
ZTEX File Reference
===================

The ZenGin Texture (.tex) file format is used to store textures with or without 
mipmap levels. This format can store uncompressed and compressed pixel formats, 
and is the proprietary file format used by the PC games Gothic I and Gothic II.

File Layout
-----------

The basic structure of a .tex file is a header, and texture data (pix/mip/pal) 
written to a binary file.  The header consists of a signature, a version value 
and a ZTEX_INFO structure.  This header contains all the information needed to 
determine the contents of the entire file. The image below shows the layout of 
a .tex file:

    +------------------------------------+
    | File Header  (ZTEX_FILE_HEADER)    |
    |   Signature  (ZTEX_FILE_SIGNATURE) |
    |   Version    (ZTEX_FILE_VERSION_0) |
    |   Info Block (ZTEX_INFO)           |
    |     Format   (ZTEX_FORMAT)         |
    |     Width of mipmap 0              |
    |     Height of mipmap 0             |
    |     Number of mipmaps (0 = none)   |
    |     Reference Width (ingame)       |
    |     Reference Height (ingame)      |
    |     Average Color (A8R8G8B8)       |
    +------------------------------------+
    | Texture data                       |
    | +--------------------------------+ |
    | | Palette data (ZTEXFMT_P8 only) | |
    | +--------------------------------+ |
    | +--------------------------------+ |
    | | Pixel data for smallest mipmap | |
    | +--------------------------------+ |
    | |              ...               | |
    | +--------------------------------+ |
    | | Pixel data mipmap 0 (biggest)  | |
    | +--------------------------------+ |
    +------------------------------------+

//TODO: more comments...


Gothic's converter behavior
===========================

The PC games Gothic I and Gothic II automatically convert Targa(2) image files 
(_work/Data/Textures/.../*.tga) into the ZTEX format and save it with the name 
_work/Data/Textures/_compiled/*-C.TEX. The ingame texture converter uses hints 
which are included in the path (case-insensitive) of the source texture (TGA):

  NOMIP    - do not generate mipmap levels
  8BIT     - alhpa: ZTEXFMT_A4R4G4B4 (6), else: ZTEXFMT_P8     (9)
  16BIT    - alpha: ZTEXFMT_A4R4G4B4 (6), else: ZTEXFMT_R5G6B5 (8)
  32BIT    - alpha: ZTEXFMT_A8B8G8R8 (2), else: ZTEXFMT_B8G8R8 (4)
  \4       - half RefWidth and RefHeight while max <= 4
  \8       - half RefWidth and RefHeight while max <= 8
  \16      - half RefWidth and RefHeight while max <= 16
  \32      - half RefWidth and RefHeight while max <= 32
  \64      - half RefWidth and RefHeight while max <= 64
  \128     - half RefWidth and RefHeight while max <= 128
  \256     - half RefWidth and RefHeight while max <= 256
  \512     - half RefWidth and RefHeight while max <= 512
  \1024    - half RefWidth and RefHeight while max <= 1024
  \_DETAIL - successive mipmaps fade to grey (weight 0.3f)

If no hints are found, the converter defaults to ZTEXFMT_DXT3 (with alpha) or 
ZTEXFMT_DXT1 (source without alpha). A Simple box filter is used for scaling.
The height and width of converted textures are always scaled to a power of 2.
Mipmaps are generated until the width or the height is lower than or equal 8.

Known Converter Bugs
--------------------

  8BIT  with mipmaps                   - lower mipmaps written into mipmap 0.
  32BIT with mipmaps and without alpha - lower mipmaps written into mipmap 0.
  32BIT with alpha                     - ZTEXFMT_A8B8G8R8 but A8R8G8B8 pixel.

