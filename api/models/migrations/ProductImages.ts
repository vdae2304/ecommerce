import * as Schema from '../Schema';

export const ProductImages = Schema.create("ProductImages")
.column({
    name: "id",
    dtype: Schema.INT(),
    nullable: false,
    auto_increment: true,
    comment: "An unique identifier for the image.",
    primary_key: true
})
.column({
    name: "productId",
    dtype: Schema.INT(),
    nullable: false,
    comment: "The product which the image belongs to.",
    foreign_key: {
        table: "Products",
        column: "id",
        on_delete: "CASCADE"
    }
})
.column({
    name: "url",
    dtype: Schema.VARCHAR(256),
    nullable: false,
    comment: "The URL of the image."
})
.column({
    name: "uploaded",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the image was uploaded."
});
