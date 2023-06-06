import * as mysql from 'mysql';

export type QueryError = mysql.MysqlError | null;
export type QueryOperator = '=' | '<>' | '<' | '>' | '<=' | '>=' | 'LIKE';

/**
 * @brief Query builder.
 */
export class QueryBuilder {
    private _connection: mysql.Connection;
    private _table: string;
    private _select: string;
    private _where: string[];
    private _having: string[];
    private _orderBy: string | null;
    private _limit: Number | null;
    private _offset: Number | null;
    private _params: any[];

    constructor(connection: mysql.Connection, table: string) {
        this._connection = connection;
        this._table = table;
        this._select = '*';
        this._where = [];
        this._having = [];
        this._orderBy = null;
        this._limit = null;
        this._offset = null;
        this._params = [];
    }

    select(...columns: string[]) {
        this._select = columns.join(', ');
        return this;
    }

    where(column: string, operator: QueryOperator, value: any) {
        this._where.push(`${column} ${operator} ?`);
        this._params.push(value);
        return this;
    }

    having(column: string, operator: QueryOperator, value: any) {
        this._having.push(`${column} ${operator} ?`);
        this._params.push(value);
        return this;
    }

    orderBy(column: string) {
        this._orderBy = column;
        return this;
    }

    limit(limit: Number) {
        this._limit = limit;
        return this;
    }

    offset(offset: Number) {
        this._offset = offset;
        return this;
    }

    query(callback?: mysql.queryCallback) {
        let query = `SELECT ${this._select} FROM ${this._table}`;
        if (this._where.length > 0) {
            query += `\nWHERE ${this._where.join(' AND ')}`;
        }
        if (this._having.length > 0) {
            query += `\nHAVING ${this._having.join(' AND ')}`;
        }
        if (this._orderBy) {
            query += `\nORDER BY ${this._orderBy}`;
        }
        if (this._limit) {
            query += `\nLIMIT ${this._limit}`
            if (this._offset) {
                query += ` OFFSET ${this._offset}`;
            }
        }
        console.log("Running query");
        console.log(query);
        console.log("with");
        console.log(this._params);
        this._connection.query(query, this._params, callback);
    }
};

/**
 * @brief Class for database.
 */
export class Database {
    private _connection: mysql.Connection;

    constructor(options) {
        this._connection = mysql.createConnection(options);
    }

    table(tableName: string): QueryBuilder {
        return new QueryBuilder(this._connection, tableName);
    }
}

const options = require('./config.json');
export const DB = new Database(options);
