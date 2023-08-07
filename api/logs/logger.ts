import * as fs from 'fs';
import * as path from 'path';

export type LoggingLevel = 'DEBUG' | 'INFO' | 'WARNING' | 'ERROR' | 'CRITICAL';

/**
 * @brief A class for logging to a file.
 */
export class FileLogger {
    private _stream: fs.WriteStream;

    constructor(filename: string) {
        this._stream = fs.createWriteStream(
            path.resolve(__dirname, filename),
            {flags: 'a', encoding: 'utf-8'});
    }

    /**
     * @brief Closes the file.
     */
    close() { this._stream.close(); }

    /**
     * @brief Logs a message.
     * 
     * @param message Message to log.
     * @param level Level of the message.
     */
    log(message: string, level: LoggingLevel) {
        this._stream.write(`[${new Date()}]:[${level}]: ${message}\n`);
    }

    /**
     * @brief Logs a message with level `DEBUG`.
     */
    debug(message: string) { this.log(message, 'DEBUG'); }

    /**
     * @brief Logs a message with level `INFO`.
     */
    info(message: string) { this.log(message, 'INFO'); }

    /**
     * @brief Logs a message with level `WARNING`.
     */
    warning(message: string) { this.log(message, 'WARNING'); }

    /**
     * @brief Logs a message with level `ERROR`.
     */
    error(message: string) { this.log(message, 'ERROR'); }

    /**
     * @brief Logs a message with level `CRITICAL`.
     */
    critical(message: string) { this.log(message, 'CRITICAL'); }
};
