import * as express from 'express';
import { Database, QueryBuilder } from '../models/database'

export const router = express.Router();

router.get('/', function (req: express.Request, res: express.Response) {
    const DB = new Database();
    DB.table('ecwid_catalogoproducto')
      .select("DISTINCT CATEGORIA1 AS name")
      .orderBy('name')
      .query(function (error, results) {
            if (error) {
                console.log(error);
                throw error;
            }
            res.json(results);
        });
});
