import * as express from 'express';
import { Database, QueryBuilder } from '../models/database'

export const router = express.Router();

router.get('/', function (request: express.Request,
                          response: express.Response) {
    const DB = new Database();
    DB.table('ecwid_catalogoproducto')
      .select("DISTINCT MARCA AS name")
      .orderBy('name')
      .query(function (error, results) {
            if (error) {
                response.status(500);
            }
            response.json(results);
        });
});
