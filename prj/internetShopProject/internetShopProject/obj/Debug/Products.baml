   M S B A M L   `   `   `  ����  internetShopProject"    internetShopProject.Products   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  Products��5      $	��450��6   $	��800��6     � 
ResizeMode$ CanMinimize=�6,   .��5      �  � RowDefinitions 5   
   �� 5      $��60 �6   �� 5	      $��* �6   �� 5
      $��50 �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��0��6.   ���#5d1d99.+�5      ��    productTextBlock#��[  6   $��Products��6B   �� 5      -   5        $��addProductButton��5        productButton#��[ 6-   $��Add Product��6T   6j   �� 5      -   5        $��removeProductButton��5      #��[ 60   $��Remove Product��6W   6p   �� 5      -   5        $��changeProductButton��5      #��[ 60   $��Change Product��6W   6p   5      �� 5   
   $ 
Horizontal=�6   $��1��6.   ���#461673.+�5      �� -   5        $��listViewForProducts��5      $	��800��6\   
������ $��0q�6�   ��61   �� *productsCollection+��5      �� 5      .R�5      �� .+�5      �� $��productNameTextBlock��6(   
�������$��16(�6W   �� 5   "   $�	Product: ��6&    �� 5   "   �6&   �� *product_Name+5      �� 5      $��productDescTextBlock��6(   
�������$��16(�6W   �� 5    "   $�Description: ��6&    �� 5!   "   �6&   �� *product_Description+5"      �� 5$      $��productCountTextBlock��6(   
�������$��16(�6X   �� 5%   "   $�Count: ��6&    �� 5&   "   �6&   �� *product_Count+5'      �� 5)      $��productPriceTextBlock��6(   
�������$��16(�6X   �� 5*   "   $�Price: ��6&    �� 5+   "   �6&   �� *product_Price+5,      5-      5.      50      51      �� 53   
   $��2��6   ���#5d1d99.+�54      �� -   5          refreshProductButton#��[ 54      $��Right=�6C   $��Refresh��6_   6q   55      56      57      