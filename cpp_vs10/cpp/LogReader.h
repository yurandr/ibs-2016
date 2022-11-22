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

	bool open(const char *filename);				// открытие файла, false - ошибка
	void close();									// закрытие файла
	bool read();									// прочитать файл в память, false - ошибка
	bool setFilter(const char *filter);				// установка фильтра для поиска строк, false - ошибка
	bool setFormarStr(const char *filter);			// установка форматной строки, false - ошибка
	bool getNextLine(char *buf, const int bufsize); // запрос очередной найденной строки, buf - буфер, bufsize - максимальная длина, false - конец файла или ошибка

private:
	std::ifstream mFile;
	boost::regex mRegEx;
	std::string mFormat;
	std::vector<std::string> mLines;
	std::vector<std::string>::const_iterator mPos;
};

#endif