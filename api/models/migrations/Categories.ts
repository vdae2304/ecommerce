import * as Schema from '../Schema';

export const Categories = Schema.create("Categories")
.column({
    name: "id",
    dtype: Schema.INT(),
    nullable: false,
    auto_increment: true,
    comment: "An unique identifier for the category.",
    primary_key: true
})
.column({
    name: "parentId",
    dtype: Schema.INT(),
    nullable: true,
    comment: "ID of the parent category (if any).",
    foreign_key: {
        table: "Categories",
        column: "id",
        on_delete: "SET NULL"
    }
})
.column({
    name: "name",
    dtype: Schema.VARCHAR(32),
    nullable: false,
    comment: "The name of the category.",
    unique_key: true
})
.column({
    name: "description",
    dtype: Schema.VARCHAR(256),
    nullable: true,
    comment: "A description for the category."
})
.column({
    name: "imageUrl",
    dtype: Schema.VARCHAR(256),
    nullable: true,
    comment: "The URL of an image for the category."
})
.column({
    name: "enabled",
    dtype: Schema.BOOLEAN(),
    nullable: false,
    default: 1,
    comment: "Whether the category is enabled or not."
})
.column({
    name: "created",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the category was created."
})
.column({
    name: "updated",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the category was last updated."
});
