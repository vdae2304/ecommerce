import * as express from 'express';
import { router as products } from './controllers/products';
import { router as categories } from './controllers/categories';
import { router as brands } from './controllers/brands';

const app = express();
const port = 3000;

app.use(function(request: express.Request,
                 response: express.Response,
                 next: express.NextFunction) {
    response.header("Access-Control-Allow-Origin", '*');
    response.header("Access-Control-Allow-Methods", 'GET POST PUT DELETE');
    next();
});

app.get('/api/v1/', function(request: express.Request,
                             response: express.Response) {
    response.json({
        "name": "ecommerce",
        "version": "1.0.0"
    });
});

app.use('/api/v1/products/', products);
app.use('/api/v1/categories/', categories);
app.use('/api/v1/brands/', brands);

app.listen(port, () => console.log(`Listening on port ${port}`));
