import * as mysql from 'mysql';
import { FileLogger } from '../logs/logger';

const options = require('./config.json');
const connection = mysql.createConnection(options);
const logger = new FileLogger('schema.log');

/**
 * @brief Begin a table manipulation.
 * 
 * @param table Name of the table to create or alter.
 * 
 * @return A Blueprint that may be used to define the table properties.
 */
export function table(table: string): Blueprint {
    return new Blueprint(table);
}

/**
 * @brief A boolean type. A value of zero is considered false. Nonzero values
 * are considered true.
 */
export function BOOLEAN() { return 'TINYINT(1)'; }

/**
 * @brief A 1-byte integer.
 * 
 * @param signed Ranges from -128 to 127 for signed values and from 0 to 255 for
 * unsigned values.
 */
export function TINYINT(signed: boolean = true) {
    return signed ? 'TINYINT' : 'TINYINT UNSIGNED';
}

/**
 * @brief A 2-bytes integer.
 * 
 * @param signed Ranges from -32768 to 32767 for signed values and from 0 to
 * 65535 for unsigned values.
 */
export function SMALLINT(signed: boolean = true) {
    return signed ? 'SMALLINT' : 'SMALLINT UNSIGNED';
}

/**
 * @brief A 4-bytes integer.
 * 
 * @param signed Ranges from -2147483648 to 2147483647 for signed values and
 * from 0 to 4294967295 for unsigned values.
 */
export function INT(signed: boolean = true) {
    return signed ? 'INT' : 'INT UNSIGNED';
}

/**
 * @brief An 8-bytes integer.
 * 
 * @param signed Ranges from -2^63 to 2^63 - 1 for signed values and from 0 to
 * 2^64 - 1 for unsigned values.
 */
export function BIGINT(signed: boolean = true) {
    return signed ? 'BIGINT' : 'BIGINT UNSIGNED';
}

/**
 * @brief A single precision (4-bytes) floating-point type.
 */
export function FLOAT() { return 'FLOAT'; }

/**
 * @brief A double precision (8-bytes) floating-point type.
 */
export function DOUBLE() { return 'DOUBLE'; }

/**
 * @brief A fixed-point type.
 * 
 * @param precision Number of significant digits.
 * @param scale Number of digits after the decimal point.
 */
export function DECIMAL(precision: number, scale: number = 0) {
    return `DECIMAL(${precision}, ${scale})`;
}

/**
 * @brief A fixed-length string.
 * 
 * @param length Number of characters to store.
 */
export function CHAR(length: number) { return `CHAR(${length})`; }

/**
 * @brief A variable-length string.
 * 
 * @param max_length Maximum number of characters to store.
 */
export function VARCHAR(max_length: number) { return `VARCHAR(${max_length})`; }

/**
 * @brief A binary large object.
 */
export function BLOB() { return 'BLOB'; }

/**
 * @brief A character string.
 */
export function TEXT() { return 'TEXT'; }

/**
 * @brief A string object with a value choosen from a list of permitted values
 * that are enumerated explicitly.
 * 
 * @param values... List of permitted values.
 */
export function ENUM(...values: string[]) {
    return `ENUM(${values.map(value => `'${value}'`).join(', ')})`;
}

/**
 * @brief A date. Date values are displayed in 'YYYY-MM-DD' format.
 */
export function DATE() { return 'DATE'; }

/**
 * @brief A time. Time values are displayed in 'hh:mm:ss[.fraction]' format.
 * 
 * @param fsp Fractional second precision. The default precision is 0.
 */
export function TIME(fsp: number = 0) { return `TIME(${fsp})`; }

/**
 * @brief A date and time combination. Datetime values are displayed in
 * 'YYYY-MM-DD hh:mm:ss[.fraction]' format.
 * 
 * @param fsp Fractional second precision. The default precision is 0.
 */
export function DATETIME(fsp: number = 0) { return `DATETIME(${fsp})`; }

/**
 * @brief A timestamp. Timestamp values are stored as the number of seconds
 * since the epoch '1970-01-01 00:00:00' UTC.
 * 
 * @param fsp Fractional second precision. The default precision is 0.
 */
export function TIMESTAMP(fsp: number = 0) { return `TIMESTAMP(${fsp})`; }

/**
 * @brief A year in 4-digit format.
 */
export function YEAR() { return 'YEAR'; }

/**
 * @brief JSON (JavaScript Object Notation) data type.
 */
export function JSON() { return 'JSON'; }

/**
 * @brief Column attributes.
 */
export interface ColumnDefinition {
    // Attributes.
    name: string;
    dtype: string;
    nullable?: boolean;
    default?: number | string | object;
    auto_increment?: boolean;
    comment?: string;
    collation?: string;
    // Constraints.
    primary_key?: boolean;
    unique_key?: boolean;
    check?: object;
    foreign_key?: ForeignKeyConstraint;
};

/**
 * @brief Allows a string to be evaluated as an expression rather than a
 * literal.
 * 
 * @param expr Expression to evaluate.
 */
export function Expression(expr: string): String { return new String(expr); }

/**
 * @brief Foreign key constraint.
 */
export interface ForeignKeyConstraint {
    table: string;
    column: string;
    on_delete?: 'CASCADE' | 'SET NULL' | 'NO ACTION' | 'SET DEFAULT';
    on_update?: 'CASCADE' | 'SET NULL' | 'NO ACTION' | 'SET DEFAULT';
};

/**
 * @brief Blueprint for table creation.
 */
export class Blueprint {
    private _table: string;
    private _columns: string[];
    private _constraints: string[];

    constructor(table: string) {
        this._table = table;
        this._columns = [];
        this._constraints = [];
    }

    /**
     * @brief Add a column.
     * 
     * @param attributes The attributes of the column to add. The following list
     * contains all available attributes:
     *  - `name`: Name of the column.
     *  - `dtype`: Data type of the column.
     *  - `nullable`: (Optional) Whether the column allows null values.
     *  - `default`: (Optional) Specify a default value for the column.
     *  - `auto_increment`: (Optional) Set an integer column as auto-increment.
     *  - `comment`: (Optional) Add a comment to the column.
     *  - `collation`: (Optional) Specify a collation for the column.
     * 
     * The following list contain all available constrains:
     *  - `primary_key`: (Optional) Set the column as primary key.
     *  - `unique_key`: (Optional) Set the column as unique.
     *  - `foreign_key`: (Optional) Add a foreign key constraint.
     * 
     * Foreign key constraints are defined by the following attributes:
     *  - `table`: Parent table.
     *  - `column`: Parent column.
     *  - `on_delete`: (Optional) Action to do when a value in the parent table
     *  is deleted. Must be one of 'CASCADE', 'SET NULL', 'NO ACTION' or
     *  'SET DEFAULT'.
     *  - `on_update`: (Optional) Action to do when a value in the parent table
     *  is updated.
     */
    add_column(attributes: ColumnDefinition): Blueprint {
        let sql = `${attributes.name} ${attributes.dtype}`;
        if (attributes.nullable) {
            sql += ' NULL';
        } else {
            sql += ' NOT NULL';
        }
        if (attributes.default) {
            if (typeof attributes.default == 'string') {
                sql += ` DEFAULT '${attributes.default}'`;
            } else {
                sql += ` DEFAULT ${attributes.default}`;
            }
        }
        if (attributes.auto_increment) {
            sql += ' AUTO_INCREMENT';
        }
        if (attributes.comment) {
            sql += ` COMMENT '${attributes.comment}'`;
        }
        if (attributes.collation) {
            sql += ` COLLATE ${attributes.collation}`;
        }
        this._columns.push(sql);

        if (attributes.primary_key) {
            let constraint = `PRIMARY KEY(${attributes.name})`;
            this._constraints.push(constraint);
        }
        if (attributes.unique_key) {
            let constraint = `UNIQUE KEY(${attributes.name})`;
            this._constraints.push(constraint);
        }
        if (attributes.check) {
            let constraint = `CHECK(${attributes.check})`;
            this._constraints.push(constraint);
        }
        if (attributes.foreign_key) {
            let constraint = `FOREIGN KEY(${attributes.name})`;
            const foreign_key = attributes.foreign_key;
            constraint += ` REFERENCES ${foreign_key.table}(${foreign_key.column})`;
            if (foreign_key.on_delete) {
                constraint += ` ON DELETE ${foreign_key.on_delete}`;
            }
            if (foreign_key.on_update) {
                constraint += ` ON UPDATE ${foreign_key.on_update}`;
            }
            this._constraints.push(constraint);
        }
        return this;
    }

    /**
     * @brief Creates the table.
     * 
     * @return A promise returning true if operation was sucessfull and false
     * otherwise.
     */
    create(): Promise<Boolean> {
        const sql = `CREATE TABLE ${this._table} (\n` +
            this._columns.concat(this._constraints).join(',\n') +
            ');';
        logger.debug(sql);

        return new Promise((resolve) => {
            connection.query(sql, (error) => {
                if (error) {
                    logger.error(error.toString());
                    return resolve(false);
                }
                return resolve(true);
            });
        });
    }

    /**
     * @brief Drops the table.
     * 
     * @return A promise returning true if operation was sucessfull and false
     * otherwise.
     */
    drop(): Promise<boolean> {
        const sql = `DROP TABLE ${this._table};`;
        logger.debug(sql);

        return new Promise((resolve) => {
            connection.query(sql, (error) => {
                if (error) {
                    logger.error(error.toString());
                    return resolve(false);
                }
                return resolve(true);
            });
        });
    }
};
