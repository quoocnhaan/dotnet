DELETE FROM Products;
DELETE FROM Categories;

DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);

INSERT INTO [dbo].[Categories] ([Name], [Description])
VALUES
('Hat', 'Various types of hats and headwear'),
('Shirt', 'Casual and formal shirts'),
('Pant', 'Long pants, jeans, and trousers'),
('Shoe', 'Different kinds of shoes for men and women'),
('Bag', 'Handbags, backpacks, and more'),
('Accessory', 'Fashion accessories and add-ons'),
('Jacket', 'Light and heavy jackets for all seasons');

INSERT INTO [dbo].[Products] 
([Name], [Description], [CategoryId], [Price], [Quantity], [Discount], [ImageUrl])
VALUES
-- Hats (CategoryId = 1)
('Baseball Cap', 'Comfortable cotton baseball cap', 1, 15.99, 50, 0.1, 'img/hat.png'),
('Beanie', 'Warm knitted beanie for winter', 1, 12.49, 40, 0.05, 'img/hat.png'),
('Sun Hat', 'Wide brim sun protection hat', 1, 18.00, 35, 0.15, 'img/hat.png'),

-- Shirts (CategoryId = 2)
('Casual T-Shirt', 'Soft cotton T-shirt', 2, 20.00, 80, 0.1, 'img/shirt.png'),
('Formal Shirt', 'Long sleeve formal shirt', 2, 35.50, 60, 0.2, 'img/shirt.png'),
('Hoodie', 'Warm hoodie with front pocket', 2, 42.00, 45, 0.12, 'img/shirt.png'),

-- Pants (CategoryId = 3)
('Blue Jeans', 'Slim fit blue jeans', 3, 45.99, 70, 0.15, 'img/pant.png'),
('Cargo Pants', 'Durable cargo pants with pockets', 3, 38.50, 55, 0.1, 'img/pant.png'),
('Joggers', 'Comfortable sports joggers', 3, 29.90, 75, 0.08, 'img/pant.png'),

-- Shoes (CategoryId = 4)
('Running Shoes', 'Lightweight performance running shoes', 4, 60.00, 40, 0.2, 'img/shoe.png'),
('Sneakers', 'Casual everyday sneakers', 4, 55.00, 50, 0.1, 'img/shoe.png'),
('Boots', 'Leather boots for winter', 4, 80.00, 30, 0.15, 'img/shoe.png'),

-- Bags (CategoryId = 5)
('Backpack', 'Durable travel backpack', 5, 40.00, 45, 0.1, 'img/bag.png'),
('Handbag', 'Fashion women’s handbag', 5, 50.00, 30, 0.12, 'img/bag.png'),
('Crossbody Bag', 'Lightweight crossbody style bag', 5, 32.00, 50, 0.08, 'img/bag.png'),

-- Accessories (CategoryId = 6)
('Wrist Watch', 'Stainless steel wrist watch', 6, 75.00, 25, 0.2, 'img/accessory.png'),
('Sunglasses', 'UV protection sunglasses', 6, 22.00, 60, 0.1, 'img/accessory.png'),
('Belt', 'Leather belt', 6, 18.00, 70, 0.05, 'img/accessory.png'),

-- Jackets (CategoryId = 7)
('Denim Jacket', 'Classic denim jacket', 7, 58.00, 40, 0.1, 'img/jacket.png'),
('Windbreaker', 'Light windbreaker jacket', 7, 49.00, 35, 0.08, 'img/jacket.png');
