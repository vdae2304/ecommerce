import * as Schema from '../Schema';

export const Products = Schema.table("Products")
.add_column({
    name: "id",
    dtype: Schema.INT(),
    nullable: false,
    auto_increment: true,
    comment: "An unique identifier for the product.",
    primary_key: true,
})
.add_column({
    name: "sku",
    dtype: Schema.CHAR(8),
    nullable: false,
    comment: "An unique identifier for the product. Similar to id, but can be manually assigned.",
    unique_key: true,
})
.add_column({
    name: "name",
    dtype: Schema.VARCHAR(32),
    nullable: false,
    comment: "The name of the product."
})
.add_column({
    name: "description",
    dtype: Schema.VARCHAR(256),
    nullable: true,
    comment: "A description for the product."
})
.add_column({
    name: "price",
    dtype: Schema.DECIMAL(6, 2),
    nullable: false,
    comment: "The price of the product."
})
.add_column({
    name: "inStock",
    dtype: Schema.INT(),
    nullable: false,
    default: 0,
    comment: "Amount of products in stock."
})
.add_column({
    name: "categoryId",
    dtype: Schema.INT(),
    nullable: true,
    comment: "The category which the product belongs to.",
    foreign_key: {
        table: "Categories",
        column: "id",
        on_delete: "SET NULL"
    }
})
.add_column({
    name: "enabled",
    dtype: Schema.BOOLEAN(),
    nullable: false,
    default: 1,
    comment: "Whether the product is enabled or not."
})
.add_column({
    name: "created",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the product was created."
})
.add_column({
    name: "updated",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the product was last updated."
});
