import * as express from 'express';
import { Database, QueryBuilder } from '../models/database'

export const router = express.Router();

type ReqQuery = {
    q?: string;
    brand?: string;
    category?: string;
    minPrice?: Number;
    maxPrice?: Number;
    limit?: Number;
    offset?: Number;
};

router.get('/', function (req: express.Request<{}, {}, {}, ReqQuery>,
                          res: express.Response) {
    const DB = new Database();
    const query: QueryBuilder = DB.table('ecwid_catalogoproducto')
        .select("REF as sku",
            "DESCRIPCION AS name",
            "FORMAT(PRECIOVENTA, 2) AS price",
            "CATEGORIA1 AS category",
            "MARCA AS brand",
            "IMAGEN AS image",);
    if (req.query.q) {
        query.where('DESCRIPCION', 'LIKE', `%${req.query.q}%`);
    }
    if (req.query.brand) {
        query.where('MARCA', '=', req.query.brand);
    }
    if (req.query.category) {
        query.where('CATEGORIA1', '=', req.query.category);
    }
    if (req.query.minPrice) {
        query.where('PRECIOVENTA', '>=', req.query.minPrice);
    }
    if (req.query.maxPrice) {
        query.where('PRECIOVENTA', '<=', req.query.maxPrice);
    }
    query.limit(req.query.limit ?? 100)
        .offset(req.query.offset ?? 0)
        .query(function (error, results) {
            if (error) {
                console.log(error);
                throw error;
            }
            res.json(results);
        });
});
