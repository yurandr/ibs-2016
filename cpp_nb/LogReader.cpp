#include "LogReader.h"

LogReader::LogReader()
{
}

LogReader::~LogReader()
{
}

bool LogReader::open(const char *filename)
{
	try
	{
		if (nullptr != filename)
		{
			mFile.open(filename);
			return mFile.good();
		}
	}
	catch (std::exception&)
	{ /* something went wrong */ }
	return false;
}

void LogReader::close()
{
	try
	{
		mFile.close();
	}
	catch (std::exception&)
	{ /* something went wrong */ }
}

bool LogReader::read()
{
	try
	{
		if (mFile.good())
		{
			mLines.clear();
			while (!mFile.eof())
			{
				std::string line;
				std::getline(mFile,line);
				mLines.push_back(line);
			}
			mPos = mLines.cbegin();
			return true;
		}
	}
	catch (std::exception&)
	{ /* something went wrong */ }
	return false;
}

bool LogReader::setFilter(const char *filter)
{
	try
	{
		if (nullptr != filter)
		{
			mRegEx = filter;
			return 0 == mRegEx.status();
		}
	}
	catch (std::exception&)
	{ /* something went wrong */ }
	return false;
}

bool LogReader::setFormarStr(const char *filter)
{
	try
	{
		if (nullptr != filter)
		{
			mFormat = filter;
			return true;
		}
	}
	catch (std::exception&)
	{ /* something went wrong */ }
	return false;
}

bool LogReader::getNextLine(char *buf, const int bufsize)
{
	try
	{
		if (nullptr != buf && bufsize > 0)
		{
			std::fill(buf, buf+bufsize, 0); // more chances to have valid null terminated string in result
			while (mPos != mLines.cend())
			{
				std::string line = *mPos++;
				boost::smatch m;
				if (boost::regex_match(line, m, mRegEx))
				{
					std::string result = boost::regex_replace(line, mRegEx, mFormat);					
					result.copy(buf, bufsize); // not safe in vs
					return true;
				}
			}
		}
	}
	catch (std::exception&)
	{ /* something went wrong */ }
	return false;
}
