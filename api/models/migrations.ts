import { Categories } from './migrations/Categories';
import { Products } from './migrations/Products';
import { ProductImages } from './migrations/ProductImages';
import { ProductTags } from './migrations/ProductTags';

Categories.execute();
Products.execute();
ProductImages.execute();
ProductTags.execute();
