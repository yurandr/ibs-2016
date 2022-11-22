#pragma once
#ifndef LOG_READER_H
#define LOG_READER_H

#include <fstream>
#include <boost/regex.hpp>

class LogReader
{
public:
	LogReader();
	~LogReader();

	bool open(const char *filename);
	void close();
	bool read();
	bool setFilter(const char *filter);
	bool setFormarStr(const char *filter);
	bool getNextLine(char *buf, const int bufsize);

private:
	std::ifstream mFile;
	boost::regex mRegEx;
	std::string mFormat;
	std::vector<std::string> mLines;
	std::vector<std::string>::const_iterator mPos;
};

#endif