#include <Irrlicht.h>
namespace edit
{
	using namespace irr;

	inline irr::SIrrlichtCreationParameters ReadConfig(const char* filename)
	{
		irr::SIrrlichtCreationParameters params;

		io::IrrXMLReader* xml= io::createIrrXMLReader(filename);
		if (!xml)
			return params;

		while ( xml->read() )
		{
			if (xml->getNodeType() )
			{
				if ( core::stringc( xml->getNodeData()).equals_ignore_case("Video"))
				{
					while ( xml->read() && !core::stringc(xml->getNodeData()).equals_ignore_case("/Video"))
					{
						if (core::stringc( xml->getNodeData()).equals_ignore_case("Driver"))
						{
							xml->read();
							if (core::stringc( xml->getNodeData()).equals_ignore_case("NULL"))
								params.DriverType = video::EDT_NULL;
							else if (core::stringc( xml->getNodeData()).equals_ignore_case("OpenGL"))
								params.DriverType = video::EDT_OPENGL;
							else if (core::stringc( xml->getNodeData()).equals_ignore_case("DirectX9"))
								params.DriverType = video::EDT_DIRECT3D9;
							else if (core::stringc( xml->getNodeData()).equals_ignore_case("Burningsvideo"))
								params.DriverType = video::EDT_BURNINGSVIDEO;
							else if (core::stringc( xml->getNodeData()).equals_ignore_case("Irrlicht"))
								params.DriverType = video::EDT_SOFTWARE;
							xml->read();
						}
						else if (core::stringc( xml->getNodeData()).equals_ignore_case("width"))
						{
							xml->read();
							params.WindowSize.Width	= atoi( xml->getNodeData());
							xml->read();
						}
						else if (core::stringc( xml->getNodeData()).equals_ignore_case("height"))
						{
							xml->read();
							params.WindowSize.Height	= atoi( xml->getNodeData());
							xml->read();
						}
						else if (core::stringc( xml->getNodeData()).equals_ignore_case("bits"))
						{
							xml->read();
							params.Bits	= atoi( xml->getNodeData());
							xml->read();
						}
						else if (core::stringc( xml->getNodeData()).equals_ignore_case("Fullscreen"))
						{
							xml->read();
							if (core::stringc( xml->getNodeData()).equals_ignore_case("True"))
							{
								params.Fullscreen = true;
							}
							xml->read();
						}
						else if (core::stringc( xml->getNodeData()).equals_ignore_case("AntiAlising"))
						{
							xml->read();
							if (core::stringc( xml->getNodeData()).equals_ignore_case("True"))
								params.AntiAlias = true;
							xml->read();
						}
						else if (core::stringc( xml->getNodeData()).equals_ignore_case("VSync"))
						{
							xml->read();
							if (core::stringc( xml->getNodeData()).equals_ignore_case("True"))
								params.Vsync = true;
							xml->read();
						}
					}//while-Schleife, in der bis </Video> gesucht wird.
				}//Wenn <Video> beginnt ...
			}//Wenn NodeType ok ist ...
		}//bis Dateiende
		return params;
	}


	inline void WriteConfig(const char* filename, const irr::SIrrlichtCreationParameters& params)
	{
		FILE* file =0;
		fopen_s(&file, filename, "wb");

		fwrite("<?xml version='1.0' encoding='UTF-8'?>\n\r", 1, strlen("<?xml version='1.0' encoding='UTF-8'?>\n\r"), file);
		fwrite("<Video>\n\r  <Driver>", 1, strlen("<Video>\n\r  <Driver>"), file);

		switch (params.DriverType)
		{
		case video::EDT_BURNINGSVIDEO: fwrite("Burningvideo", 1, strlen("Burningvideo"), file); break;
		case video::EDT_DIRECT3D9: fwrite("DirectX9", 1, strlen("DirectX9"), file); break;
		case video::EDT_OPENGL: fwrite("OpenGL", 1, strlen("OpenGL"), file); break;
		case video::EDT_SOFTWARE: fwrite("Irrlicht", 1, strlen("Irrlicht"), file); break;
		case video::EDT_NULL: fwrite("NULL", 1, strlen("NULL"), file); break;
		}
		fwrite("</Driver>\n\r  <width>", 1, strlen("</Driver>\n\r  <width>"), file);

		char buf[32]="";
		_itoa_s(params.WindowSize.Width, buf, 10);
		fwrite(buf, 1, strlen(buf), file);
		fwrite("</width>\n\r  <height>", 1, strlen("</width>\n\r  <height>"), file);

		_itoa_s(params.WindowSize.Height, buf, 10);
		fwrite(buf, 1, strlen(buf), file);
		fwrite("</height>\n\r  <bits>", 1, strlen("</height>\n\r  <bits>"), file);

		_itoa_s(params.Bits, buf, 10);
		fwrite(buf, 1, strlen(buf), file);
		fwrite("</bits>\n\r  <fullscreen>", 1, strlen("</bits>\n\r  <fullscreen>"), file);


		if (params.Fullscreen)
			fwrite("True</fullscreen>\n\r  <AntiAlising>", 1, strlen("True</fullscreen>\n\r  <AntiAlising>"), file);
		else
			fwrite("False</fullscreen>\n\r  <AntiAlising>", 1, strlen("False</fullscreen>\n\r  <AntiAlising>"), file);

		if (params.AntiAlias)
			fwrite("True</AntiAlising>\n\r  <VSync>", 1, strlen("True</AntiAlising>\n\r  <VSync>"), file);
		else
			fwrite("False</AntiAlising>\n\r  <VSync>", 1, strlen("False</AntiAlising>\n\r  <VSync>"), file);

		if (params.Vsync)
			fwrite("True</VSync>\n\r</Video>", 1, strlen("True</VSync>\n\r</Video>"), file);
		else
			fwrite("False</VSync>\n\r</Video>", 1, strlen("False</VSync>\n\r</Video>"), file);
	}
}//namespace edit