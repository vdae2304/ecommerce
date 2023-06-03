import * as express from 'express';
import { router as products } from './controllers/products';
import { router as categories } from './controllers/categories';
import { router as brands } from './controllers/brands';

const app = express();
const port = 3000;

app.use(function(req: express.Request,
                 res: express.Response,
                 next: express.NextFunction) {
    res.header("Access-Control-Allow-Origin", '*');
    res.header("Access-Control-Allow-Methods", 'GET POST PUT DELETE');
    next();
});

app.get('/api/v1/', function(req: express.Request, res: express.Response) {
    res.json({
        "name": "ecommerce",
        "version": "1.0.0"
    });
});

app.use('/api/v1/products/', products);
app.use('/api/v1/categories/', categories);
app.use('/api/v1/brands/', brands);

app.listen(port, () => {
    console.log(`Listening on port ${port}`);
});
