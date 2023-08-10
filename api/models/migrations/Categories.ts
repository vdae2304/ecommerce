import * as Schema from '../Schema';

export const Categories = Schema.table("Categories")
.add_column({
    name: "id",
    dtype: Schema.INT(),
    nullable: false,
    auto_increment: true,
    comment: "An unique identifier for the category.",
    primary_key: true
})
.add_column({
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
.add_column({
    name: "name",
    dtype: Schema.VARCHAR(32),
    nullable: false,
    comment: "The name of the category.",
    unique_key: true
})
.add_column({
    name: "description",
    dtype: Schema.VARCHAR(256),
    nullable: true,
    comment: "A description for the category."
})
.add_column({
    name: "imageUrl",
    dtype: Schema.VARCHAR(256),
    nullable: true,
    comment: "The URL of an image for the category."
})
.add_column({
    name: "enabled",
    dtype: Schema.BOOLEAN(),
    nullable: false,
    default: 1,
    comment: "Whether the category is enabled or not."
})
.add_column({
    name: "created",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the category was created."
})
.add_column({
    name: "updated",
    dtype: Schema.DATETIME(),
    nullable: false,
    default: Schema.Expression("CURRENT_TIMESTAMP"),
    comment: "The date and time the category was last updated."
});
