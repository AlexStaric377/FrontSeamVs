   M S B A M L   `   `   `  ����c  _System.Windows.Interactivity, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity  5   	   E AFrontSeam, Version=1.0.1.59, Culture=neutral, PublicKeyToken=null%clr-namespace:FrontSeam	FrontSeam 5	   	      FrontSeam.WaitWindow   5   	   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   U QWpfAnimatedGif, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9e7cd3b544a090dc,gif"http://wpfanimatedgif.codeplex.com  5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   [intrPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5   	   #localclr-namespace:FrontSeam  5	   	   -   5        $��
WindowWait��5        � Title$  
WaitWindow��6	    � WindowState$ Normal=�6.    � AllowsTransparency$
 True��6C    � WindowStyle$
 None=�6]    � WindowStartupLocation$ CenterScreen=�6p    � ShowInTaskbar$ False��5   
   $	��120��6"   $	��120��6/   $��0q�6;   $��Stretch=�6F   $��Stretch=�6d    J� Opacity$	 1.0Q�6�    � Topmost$
 True��5      ��6	   Z� .��5      � .+�5   
   �� -   5         ��Name$ 	BorderWin��5      
�������
�������$	 1.0Q�6K   $��0,0,0,0q�6Z   	 �� CornerRadius$	 30,30,30,30��6t   $	��120��6�   $	��120��6�   ._�5      �� -   5        $
��Wait��5   0   $	��100��6   $	��100��6!   $D��>pack://application:,,,/FrontSeam;component/Images/loading9.gif��6=   
 � Cursor$

 Hand��6�   z�5      �� 5      -   5        
rotate�� �� CenterX$ 50Q�5       �� CenterY$ 50Q�5       � Triggers 5      :� 5       :� RoutedEvent$ Loaded��6#   .[�5      �� .��5      ��  g� RepeatBehavior$ Forever��6)   .��5   "   W�  �� 
TargetName$ rotate��62    �� TargetProperty$ Angle�5   &   	 W� By 3605   &    g� Duration$ 0:0:2D�6/   5      5      5      5!      5"      5#      5$      