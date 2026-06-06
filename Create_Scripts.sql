CREATE TABLE categories
(
    category_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    category_name VARCHAR(200) NOT NULL,
    description TEXT,

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_date TIMESTAMP NULL
);

CREATE UNIQUE INDEX idx_categories_name
ON categories(category_name);

------------------------------------------------------------------------


CREATE TABLE subcategories
(
    subcategory_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    category_id UUID NOT NULL,

    subcategory_name VARCHAR(200) NOT NULL,
    description TEXT,

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_date TIMESTAMP NULL,

    CONSTRAINT fk_subcategories_category
        FOREIGN KEY(category_id)
        REFERENCES categories(category_id)
        ON DELETE RESTRICT
);

CREATE INDEX idx_subcategories_category
ON subcategories(category_id);

CREATE UNIQUE INDEX idx_subcategory_name_per_category
ON subcategories(category_id, subcategory_name);


------------------------------------------------------

CREATE TABLE products
(
    product_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    subcategory_id UUID NOT NULL,

    product_name VARCHAR(300) NOT NULL,

    description TEXT,

    brand VARCHAR(200),

    price NUMERIC(18,2) NOT NULL,

    discount_price NUMERIC(18,2),

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_date TIMESTAMP NULL,

    CONSTRAINT fk_products_subcategory
        FOREIGN KEY(subcategory_id)
        REFERENCES subcategories(subcategory_id)
        ON DELETE RESTRICT
);

CREATE UNIQUE INDEX idx_products_sku
ON products(sku);

CREATE INDEX idx_products_subcategory
ON products(subcategory_id);

CREATE INDEX idx_products_name
ON products(product_name);


----------------------------------------------------------------------


CREATE TABLE product_images
(
    image_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    product_id UUID NOT NULL,

    image_url TEXT NOT NULL,

    display_order INTEGER NOT NULL DEFAULT 1,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_product_images_product
        FOREIGN KEY(product_id)
        REFERENCES products(product_id)
        ON DELETE CASCADE
);

CREATE INDEX idx_product_images_product
ON product_images(product_id);

-----------------------------------------------------

CREATE EXTENSION IF NOT EXISTS pgcrypto;

select * From pg_extension;

SELECT gen_random_uuid();

----------------------------------------------------------------

CREATE TABLE product_variants
(
    variant_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    product_id UUID NOT NULL,

    sku VARCHAR(100) NOT NULL,

    variant_name VARCHAR(200) NOT NULL,

    size_ml INTEGER,

    pack_size INTEGER NOT NULL DEFAULT 1,

    weight_grams NUMERIC(10,2),

    mrp NUMERIC(18,2) NOT NULL,

    barcode VARCHAR(100),

    is_default BOOLEAN NOT NULL DEFAULT FALSE,

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    updated_date TIMESTAMP NULL,

    CONSTRAINT fk_product_variants_product
        FOREIGN KEY (product_id)
        REFERENCES products(product_id)
        ON DELETE CASCADE,

    CONSTRAINT uq_product_variants_sku
        UNIQUE (sku)
);

-----------------------------------------------------------------------------------


CREATE TABLE variant_discounts
(
    discount_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    variant_id UUID NOT NULL,

    discount_percentage NUMERIC(5,2),

    start_date TIMESTAMP,

    end_date TIMESTAMP,

    is_active BOOLEAN DEFAULT TRUE
);
---------------------------------------------------------------------------------------

CREATE TABLE inventory
(
    variant_id UUID NOT NULL,

    total_stock INTEGER NOT NULL DEFAULT 0,

    reserved_stock INTEGER NOT NULL DEFAULT 0,

    available_stock INTEGER NOT NULL DEFAULT 0,

    reorder_level INTEGER NOT NULL DEFAULT 10,

    last_stock_updated TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_inventory_variant
        FOREIGN KEY (variant_id)
        REFERENCES product_variants(variant_id)
        ON DELETE CASCADE
);

---------------------------------------------------------------------------------------------

CREATE TABLE orders
(
    order_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    order_number VARCHAR(50) NOT NULL UNIQUE,

    customer_id UUID NOT NULL,

    order_status VARCHAR(50) NOT NULL,

    subtotal_amount NUMERIC(18,2) NOT NULL,

    discount_amount NUMERIC(18,2) NOT NULL DEFAULT 0,

    shipping_amount NUMERIC(18,2) NOT NULL DEFAULT 0,

    tax_amount NUMERIC(18,2) NOT NULL DEFAULT 0,

    total_amount NUMERIC(18,2) NOT NULL,

    payment_status VARCHAR(50) NOT NULL,

    notes TEXT,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    updated_date TIMESTAMP NULL

    CONSTRAINT fk_orders_customer
        FOREIGN KEY(customer_id)
        REFERENCES users(user_id)
);

-------------------------------------------------------------------------------------------------------

CREATE TABLE order_items
(
    order_item_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    order_id UUID NOT NULL,

    variant_id UUID NOT NULL,

    product_name VARCHAR(300) NOT NULL,

    variant_name VARCHAR(200),

    sku VARCHAR(100) NOT NULL,

    quantity INTEGER NOT NULL,

    unit_price NUMERIC(18,2) NOT NULL,

    discount_amount NUMERIC(18,2) NOT NULL DEFAULT 0,

    total_amount NUMERIC(18,2) NOT NULL,

    CONSTRAINT fk_order_items_order
        FOREIGN KEY(order_id)
        REFERENCES orders(order_id)
        ON DELETE CASCADE,

    CONSTRAINT fk_order_items_variant
        FOREIGN KEY(variant_id)
        REFERENCES product_variants(variant_id)
);

--------------------------------------------------------------------------------------


CREATE TABLE payment_transactions
(
    payment_transaction_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    order_id UUID NOT NULL,

    payment_provider VARCHAR(50) NOT NULL,

    provider_order_id VARCHAR(200),

    provider_payment_id VARCHAR(200),

    provider_signature VARCHAR(500),

    transaction_amount NUMERIC(18,2) NOT NULL,

    payment_method VARCHAR(50) NOT NULL,

    transaction_status VARCHAR(50) NOT NULL,

    gateway_response TEXT,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    updated_date TIMESTAMP NULL,

    CONSTRAINT fk_payment_transaction_order
        FOREIGN KEY(order_id)
        REFERENCES orders(order_id)
);

----------------------------------------------------------------------------------------------------

CREATE TABLE users
(
    user_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    first_name VARCHAR(100),

    last_name VARCHAR(100),

    email VARCHAR(255),

    mobile_number VARCHAR(20),

    is_email_verified BOOLEAN NOT NULL DEFAULT FALSE,

    is_mobile_verified BOOLEAN NOT NULL DEFAULT FALSE,

    is_active BOOLEAN NOT NULL DEFAULT TRUE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    updated_date TIMESTAMP NULL


);
-----------------------------------------------------------------------------------------------------

CREATE TABLE customer_addresses
(
    address_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_id UUID NOT NULL,

    address_type VARCHAR(50),

    full_name VARCHAR(200) NOT NULL,

    mobile_number VARCHAR(20) NOT NULL,

    address_line1 VARCHAR(500) NOT NULL,

    address_line2 VARCHAR(500),

    landmark VARCHAR(200),

    city VARCHAR(100) NOT NULL,

    state VARCHAR(100) NOT NULL,

    postal_code VARCHAR(20) NOT NULL,

    country VARCHAR(100) NOT NULL DEFAULT 'India',

    is_default BOOLEAN NOT NULL DEFAULT FALSE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    updated_date TIMESTAMP NULL,

    CONSTRAINT fk_customer_addresses_user
        FOREIGN KEY (user_id)
        REFERENCES users(user_id)
        ON DELETE CASCADE
);
------------------------------------------------------------------------------------------------------
CREATE TABLE order_addresses
(
    order_address_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    order_id UUID NOT NULL UNIQUE,

    full_name VARCHAR(200) NOT NULL,

    phone_number VARCHAR(20) NOT NULL,

    address_line1 VARCHAR(500) NOT NULL,

    address_line2 VARCHAR(500),

    city VARCHAR(100) NOT NULL,

    state VARCHAR(100) NOT NULL,

    postal_code VARCHAR(20) NOT NULL,

    country VARCHAR(100) NOT NULL,

    CONSTRAINT fk_order_address_order
        FOREIGN KEY(order_id)
        REFERENCES orders(order_id)
        ON DELETE CASCADE
);

-------------------------------------------------------------------------------------------

CREATE TABLE otp_transactions
(
    otp_transaction_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    mobile_number VARCHAR(20) NOT NULL,

    otp_code VARCHAR(10) NOT NULL,

    purpose VARCHAR(50) NOT NULL,

    is_used BOOLEAN NOT NULL DEFAULT FALSE,

    expires_at TIMESTAMP NOT NULL,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);

-------------------------------------------------------------------------------------------------

CREATE TABLE carts
(
    cart_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    user_id UUID NOT NULL,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_cart_user
        FOREIGN KEY(user_id)
        REFERENCES users(user_id)
        ON DELETE CASCADE
);

---------------------------------------------------------------


CREATE TABLE product_reviews
(
    review_id UUID PRIMARY KEY DEFAULT gen_random_uuid(),

    product_id UUID NOT NULL,

    user_id UUID NOT NULL,

    rating INTEGER NOT NULL,

    review_title VARCHAR(200),

    review_text TEXT,

    is_approved BOOLEAN NOT NULL DEFAULT FALSE,

    created_date TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
);