import * as express from 'express';
import { DB, QueryBuilder } from '../models/database'

export const router = express.Router();

type ReqQuery = {
    q?: string;
    brand?: string;
    category?: string;
    minPrice?: Number;
    maxPrice?: Number;
    sortBy?: "name-asc" | "name-desc" | "price-asc" | "price-desc";
    limit?: Number;
    offset?: Number;
};

router.get('/', function (request: express.Request<{}, {}, {}, ReqQuery>,
                          response: express.Response) {
    const query: QueryBuilder =
    DB.table('ecwid_catalogoproducto')
      .select("REF as sku",
            "DESCRIPCION AS name",
            "FORMAT(PRECIOVENTA, 2) AS price",
            "CATEGORIA1 AS category",
            "MARCA AS brand",
            "IMAGEN AS image",
            "CAST(HABILITADO AS INTEGER) AS available",
            "EXISTENCIA as stock");
    if (request.query.q) {
        query.where('DESCRIPCION', 'LIKE', `%${request.query.q}%`);
    }
    if (request.query.brand) {
        query.where('MARCA', '=', request.query.brand);
    }
    if (request.query.category) {
        query.where('CATEGORIA1', '=', request.query.category);
    }
    if (request.query.minPrice) {
        query.where('PRECIOVENTA', '>=', request.query.minPrice);
    }
    if (request.query.maxPrice) {
        query.where('PRECIOVENTA', '<=', request.query.maxPrice);
    }
    switch (request.query.sortBy) {
    case "name-asc":
        query.orderBy('DESCRIPCION ASC');
        break;
    case "name-desc":
        query.orderBy('DESCRIPCION DESC');
        break;
    case "price-asc":
        query.orderBy('PRECIOVENTA ASC');
        break;
    case "price-desc":
        query.orderBy('PRECIOVENTA DESC');
        break;
    }
    query.limit(request.query.limit ?? 50)
         .offset(request.query.offset ?? 0)
         .query(function (error, results) {
            if (error) {
                console.log(error);
                response.sendStatus(500);
            } else {
                response.json(results);
            }
        });
});

router.get('/:id', function(request: express.Request,
                            response: express.Response) {
    DB.table('ecwid_catalogoproducto')
      .select("REF as sku",
        "DESCRIPCION AS name",
        "FORMAT(PRECIOVENTA, 2) AS price",
        "CATEGORIA1 AS category",
        "MARCA AS brand",
        "IMAGEN AS image",
        "CAST(HABILITADO AS INTEGER) AS available",
        "EXISTENCIA as stock")
      .where('REF', '=', request.params.id)
      .limit(1)
      .query(function (error, results) {
            if (error) {
                console.log(error);
                response.sendStatus(500);
            } else if (results.length == 0) {
                response.sendStatus(404);
            } else {
                response.json(results);
            }
      });
});
