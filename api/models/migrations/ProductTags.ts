import * as Schema from '../Schema';

export const ProductTags = Schema.table("ProductTags")
.add_column({
    name: "id",
    dtype: Schema.INT(),
    nullable: false,
    auto_increment: true,
    comment: "An unique identifier for the tag.",
    primary_key: true
})
.add_column({
    name: "productId",
    dtype: Schema.INT(),
    nullable: false,
    comment: "The product which the tag belongs to.",
    foreign_key: {
        table: "Products",
        column: "id",
        on_delete: "CASCADE"
    }
})
.add_column({
    name: "name",
    dtype: Schema.VARCHAR(32),
    nullable: false,
    comment: "The name of the tag."
})
.add_column({
    name: "value",
    dtype: Schema.VARCHAR(256),
    nullable: false,
    comment: "The value of the tag."
})
.add_column({
    name: "created",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the tag was created."
});
