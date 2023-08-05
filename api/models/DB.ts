import * as mysql from 'mysql';

export type QueryOperator = '=' | '<>' | '!=' | '<' | '>' | '<=' | '>='
    | 'IS' | 'IS NOT' | 'LIKE' | 'NOT LIKE';

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

    /**
     * @brief Append a where clause to the query.
     * 
     * @param column Column to filter by.
     * @param operator (Optional) Comparison operator. Defaults to '=' if not
     * provided.
     * @param value Value to filter by.
     */
    where(column: string, value: any): this;
    where(column: string, operator: QueryOperator, value: any): this;

    where(column: string, operator: any, value?: any) {
        if (value === undefined) {
            operator = '=';
            value = operator;
        }
        this._where.push(`${column} ${operator} ?`);
        this._params.push(value);
        return this;
    }

    /**
     * @brief Append a `<column> BETWEEN <minValue> AND <maxValue>` where clause
     * to the query.
     */
    whereBetween(column: string, minValue: any, maxValue: any) {
        this._where.push(`${column} BETWEEN ? AND ?`);
        this._params.push(minValue, maxValue);
        return this;
    }

    /**
     * @brief Append a `<column> NOT BETWEEN <minValue> AND <maxValue>` where
     * clause to the query.
     */
    whereNotBetween(column: string, minValue: any, maxValue: any) {
        this._where.push(`${column} NOT BETWEEN ? AND ?`);
        this._params.push(minValue, maxValue);
        return this;
    }

    /**
     * @brief Append a `<column> IN <values>` where clause to the query.
     */
    whereIn(column: string, values: any[]) {
        this._where.push(`${column} IN (${values.map(_ => '?').join(', ')})`);
        this._params.push(...values);
        return this;
    }

    /**
     * @brief Append a `<column> NOT IN <values>` where clause to the query.
     */
    whereNotIn(column: string, values: any[]) {
        this._where.push(`${column} NOT IN (${values.map(_ => '?').join(', ')})`);
        this._params.push(...values);
        return this;
    }

    /**
     * @brief Append a `<column> IS NULL` where clause to the query.
     */
    whereNull(column: string) {
        this._where.push(`${column} IS NULL`);
        return this;
    }

    /**
     * @brief Append a `<column> IS NOT NULL` where clause to the query.
     */
    whereNotNull(column: string) {
        this._where.push(`${column} IS NOT NULL`);
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

    /**
     * @brief Return a string containing the query.
     */
    toString() {
        let sql = `SELECT ${this._select}\nFROM ${this._table}`;
        if (this._where.length > 0) {
            sql += `\nWHERE ${this._where.join(' AND ')}`;
        }
        if (this._having.length > 0) {
            sql += `\nHAVING ${this._having.join(' AND ')}`;
        }
        if (this._orderBy) {
            sql += `\nORDER BY ${this._orderBy}`;
        }
        if (this._limit) {
            sql += `\nLIMIT ${this._limit}`
            if (this._offset) {
                sql += ` OFFSET ${this._offset}`;
            }
        }
        return sql;
    }

    /**
     * @brief
     */
    async count() {
        let sql = `SELECT COUNT(*) FROM ${this._table}`;
        if (this._where.length > 0) {
            sql += `\nWHERE ${this._where.join(' AND ')}`;
        }
        if (this._having.length > 0) {
            sql += `\nHAVING ${this._having.join(' AND ')}`;
        }
        console.log(sql);
        return this.execute(sql, this._params);
    }

    /**
     * @brief
     */
    async find() {
        let sql = this.toString();
        console.log(sql);
        return this.execute(sql, this._params);
    }

    private async execute(sql: string, params?: any[]) {
        return new Promise((resolve, reject) => {
            this._connection.query(sql, params,
                (error: mysql.MysqlError | null, results: any) => {
                    return error ? reject(error) : resolve(results);
                });
        });
    }
};

/**
 * @brief Begin a query. Return a QueryBuilder instance for the given table.
 * 
 * @param name Name of the table.
 */
export function table(name: string, connection?: mysql.Connection) {
    if (!connection) {
        const options = require('./config.json');
        connection = mysql.createConnection(options);
    }
    return new QueryBuilder(connection, name);
}
