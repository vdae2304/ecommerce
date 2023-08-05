import * as fs from 'fs';

export type LoggingLevel = 'DEBUG' | 'INFO' | 'WARNING' | 'ERROR';

/**
 * @brief A class for logging into a file.
 */
export class FileLogger {
    private _stream: fs.WriteStream;

    constructor(path: string) {
        this._stream = fs.createWriteStream(path, {flags: 'a'});
    }

    /**
     * @brief Logs a message.
     * 
     * @param message Message to log.
     * @param level Level of the message.
     */
    log(message: string, level: LoggingLevel) {
        this._stream.write(`[${new Date()}]:[${level}] ${message}\n`);
    }

    /**
     * @brief Logs a debug message.
     */
    debug(message: string) {
        this.log(message, 'DEBUG');
    }

    /**
     * @brief Logs an informative message.
     */
    info(message: string) {
        this.log(message, 'INFO');
    }

    /**
     * @brief Logs a warning message.
     */
    warning(message: string) {
        this.log(message, 'WARNING');
    }

    /**
     * @brief Logs an error message.
     */
    error(message: string) {
        this.log(message, 'ERROR');
    }
};
