   M S B A M L   `   `   `  ����M  IServerManagement, Version=11.7.17.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:ServerManagement.ViewServerManagement.View  5      U QMahApps.Metro, Version=1.5.0.23, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      ;"clr-namespace:ServerManagement.VMLServerManagement.VML  5      )    #ServerManagement.View.AccountUpdate   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:ServerManagement.View     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5      0w"clr-namespace:ServerManagement.VML     5      .��5      � $��Center=�6   $��5 0 5 0q�6&     � RowDefinitions  5   
   �� 5      $��50 �6   6*   �� 5      $��50 �6   6*   �� 5      $��50 �6   6*   �� 5      $��50 �6   6*   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5   
   �� -   5         ��Name$ txtUsername��5      $��0��6'   $
��Auto��64   $��Center=�6A   *  $MahApps.Metro.Controls.TextBoxHelper   UseFloatingWatermark .    	Watermark$ Username��5       �� 
IsReadOnly . $��	30 0 30 0q�5      ��5      ��  �� Path$ Username� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      G� 5   
   -   5         G�Name$ passwordBox��5      $��1��6+   $
��Auto��68   $��Center=�6E    . $ New Password��5       "  MetroButtonRevealedPasswordBox#��[  5      $��	30 0 30 0q�5      )   #ServerManagement.VML.PasswordHelper	   Attach	 . 
   Password
 5!      �� $ Password�$ TwoWay=�$ PropertyChanged=� J� InputBindings 5"      �� 5#      
 �� Key$ Return��6F    �� Command 6   �� *UpdateAccountCommand+6F   5%      G� 5&   
   -   5        $ passwordConfirmBox��5&      $��2��62   $
��Auto��6?   $��Center=�6L    . $ Confirm New Password��5(      #��[  5)      $��	30 0 30 0q�5*      	 . 
 5,      �� $ ConfirmPassword�$ TwoWay=�$ PropertyChanged=� 5-      �� 5.      $ Return��6F    6   �� *UpdateAccountCommand+6F   50      �� 51   
   -   5         ��Name$ roleComboBox��55      $��3��51      $
��Auto��6     . $ Please select an Role...��53      $��	30 0 30 0q�54       �� SelectedItem 56      �� $ CurrentRole�$ TwoWay=�$ PropertyChanged=�6q   �� 57   
   $��4��6   $��Active��6t   $��
30 20 30 0q�58       d� 	IsChecked 57       �� $ Active�$ TwoWay=�$ PropertyChanged=�58      �� 59   
   $��5��6,   $��Center=�5:      $
��Wrap=�6R   $��Center=�6f   $��13(�5;      $��DemiBold"�6"   ��59      �� *Status+��5:   /   �� *StatusColor+5<      �� 5=   
   $��6��6   $��Update��6     SquareButtonStyle#���  6/   $��Center=�5@      $	��100��5A      $��13(�6   $��	0 30 0 10q�6+   ��5>      �� *UpdateAccountCommand+��5?      ��  �� ElementName$ passwordBox��5A   +   5B      5C      