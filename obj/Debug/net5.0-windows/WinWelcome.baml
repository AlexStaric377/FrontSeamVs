   M S B A M L   `   `   `  ����E  AFrontSeam, Version=1.0.1.59, Culture=neutral, PublicKeyToken=null%clr-namespace:FrontSeam	FrontSeam  5	   	       FrontSeam.WinWelcome   5      W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   #localclr-namespace:FrontSeam  5	   	     � Title$  
WinWelcome��5   	   $	��570��6   $
��1070��6)    � WindowState$ Normal=�66    � AllowsTransparency$
 True��6K    � WindowStyle$
 None=�6e    � WindowStartupLocation$ CenterOwner=�6x    � ShowInTaskbar$
 True��5   
   $��0q�6"   $��Stretch=�6-   $��Stretch=�6K   $��Microsoft Sans Serif)�6g    � DataContext 5      #   FrontSeam.ViewModelWinWelcome  5   
   .��5      � $	��520��6   $
��1050��6   $��Center=�6%   .+�5   
   �� -   5         ��Name$ 	ButtonWin��5      $��Center=�6)   $	��500��6D   $
��1000��6Q    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      	   Color$	 	#FF1D1504��6#   
   
BlurRadius$
 45Q�65      ShadowDepth$ 20Q�6E      Opacity$	 0.5Q�6V   6g   .+�5      �� -   5         ��Name$ 	BorderWin��5      ���	#FFE7E8EA���	#FFE7E8EA$	��500��6W   $	��990��6d   $��0,0,0,0q�6q    �� CornerRadius$ 15,15,15,15��6�   ._�5      �  � ColumnDefinitions 5      �� 5      $
��250* �6+   �� 5      $
��730* �6+    � RowDefinitions 5      �� 5      $	��40* �6(   �� 5      $
��140* �6(   �� 5       $
��140* �6(   �� 5!      $
��140* �6(   �� 5"      $
��140* �6(   �� 5#      $	��50* �6(   .+�5(      �� -   5        $��	Zagolovok��5(      $����Шановні користувачі нижче наведено перелік ваших статусів, які ви можете використовувати спілкуючись з системою ��6.   $��0��6�   $��30��6�   $	��900��6�   $��16(�6�   $��Microsoft Sans Serif)�6�   $	��Top=�6�   $��Center=�6  $��Center=�69  $��2��6V  �� 5*      ���	#FF979595$��1,1,1,1q�65   $��0��6O   $��1��6\   $��30��6m   $��30��6y   $��0,0,10,0q�6�   $��Right=�6�   $��Center=�6�   ._�5+      �� ���	#FFE7E8EA$��30��6Y   $��30��6e   $��Center=�6q   ���	#FFCED7F7��6!   �� *CloseResult+.��5,      �� $C��=pack://application:,,,/FrontSeam;component/Images/delete2.png��6%   $��24��6m   $��24��6y   6�   5-      5.      �� 56      -   5         ��Name$ NameInterview��56       $��1��67   $��1��6D   $
��Wrap=�6T   $��14(�6h   $��  Гість��6v   $��Microsoft Sans Serif)�6�   $	��670��6�   $
��Left=�6�    �� TextAlignment$
 Left=�6�   $��80��6�   �� 58      -   5        $ ListInterview��58       $��4��67   $��1��6D   $��Зміст скарги��6T   $��Microsoft Sans Serif)�6h   $��14(�6�   $��20��6�   $��90��6�   $��Right=�6�   $��Center=�6�   $��0,0,50,0q�6�   $y�Visible=�6�   �� 59      -   5         ��Name$ FolderListInterview��59      $C��=pack://application:,,,/FrontSeam;component/Images/folder_.png��69   $��1��6�   $��4��6�   $��25��6�   $��Center=�6�   $��Right=�6�   $��0,0,20,0q�6�   $y�Visible=�6�   *  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5:      +  %Microsoft.Xaml.Behaviors.EventTrigger  5;         	EventName$ MouseLeftButtonDown��6-   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5<   "   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     CommandParameter$  ��6�      Command 68   ��  �� ElementName$ GridResultInterview�� �� Path$( "DataContext.ReadColectionIntreview�6�   5=      5?      �� 5@      -   5        $ NameDiagnoz��5@       $��2��65   $��1��6B   $
��Wrap=�6R   $��80��6h   $	��670��6t   $��  Паціент��6�   $��Microsoft Sans Serif)�6�   $
��Left=�6�   $��Center=�6�   $��14(�6�   �� 5B      -   5        $ 	ShortOPis��5B       $��3��63   $��1��6@   $��  Лікар
 ��6P   $��Microsoft Sans Serif)�6e   $��14(�6�   $��80��6�   $	��670��6�   $
��Left=�6�   $��Center=�6�   �� 5D      -	   5        $ NameRecomen��5D       $��1��65   $��4��6E   $%��  Адміністратор
  ��6R   $��Microsoft Sans Serif)�6p   $
��Wrap=�6�   $��14(�6�   $��60��6�   $	��600��6�   $
��Left=�6�   $	��Top=�6�   �� 5G      $��5��6   $��1��6+   $��28��6;   $	��100��6G   ���	#FFCED7F7���	#FFCED7F7$��Center=�6�   $��Center=�6�   $��1,1,1,1q�5H      $ 15,15,15,15��6:    � Cursor$
 Hand��6U   $��	0,0,100,0q�6c    5I        5J      $ MouseLeftButtonDown��6-   . 5K   "     $  ��6Y    68   �� *CloseWelcome+6Y   5L      ._�5N      �� $��2��6    $��28��60   $��14(�6<   $��80��6K   $��Далі��6V   ���	#FFCED7F7$��Center=�5O      $��Center=�64   $��Center=�6Q   $��Center=�6v   $��Microsoft Sans Serif)�6�   5P      5Q      5U      5W      5X      5Y      5Z      