#include <iostream>
#include "LogReader.h"

int main(int argc, const char * argv[])
{
	try
	{
		if (argc < 4)
		{
			std::cout << "cpp.exe <filename> <filter> <format>" << std::endl;
			return 0;
		}

		const char* filename = argv[1];
		const char* filter = argv[2];
		const char* format = argv[3];

		LogReader reader;
		if (reader.open(filename))
		{
			if (reader.read())
			{
				if (reader.setFilter(filter) && reader.setFormarStr(format))
				{
					std::array<char, 80> buffer = {0};
					while(reader.getNextLine(buffer.data(), buffer.size() /* vs: warning is here, because of possible data loss, should I cast argument? */))
						std::cout << std::string(buffer.cbegin(), buffer.cend()) << std::endl;
				}
			}
			reader.close();
		}
	}
	catch (std::exception& e)
	{
		std::cerr << e.what() << std::endl;
	}
	catch (...)
	{
		std::cerr << "something bad happened" << std::endl;
	}
	return 0;
}