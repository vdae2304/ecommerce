import * as express from 'express';
import { DB, QueryBuilder } from '../models/database'

export const router = express.Router();

router.get('/', function (request: express.Request,
                          response: express.Response) {
    DB.table('ecwid_catalogoproducto')
      .select("DISTINCT MARCA AS name")
      .orderBy('name')
      .query(function (error, results) {
            if (error) {
                console.log(error);
                response.sendStatus(500);
            } else {
                response.json(results);
            }
        });
});
