# How To Use


## The DL

Loading the SFO File 

```c#
  var psfo = new Param_SFO.PARAM_SFO("Location of SFO");
```

Check which console the sfo is for 

```c#
  Param_SFO.PARAM_SFO.Playstation version = psfo.PlaystationVersion;
```

A Couple of standard paramaters can be gathered from the SFO 


| Paramater | Description |
| --- | --- |
| APP_VER | Retruns the app version represented as a string value |
| Attribute | Attribute from the Param file |
| Category | Gets the category from the param file (differs per console) |
| ContentID |  |
| DataType |  |
| Detail |  |
| PlaystationVersion |  Retruns what console this sfo came from |
| Title | Gets the Title Of the Current Game |
| TitleID | Returns the TITLEID before the first split ('-') |
| TITLEID | Gets the Title ID Of the Current Game |


Getting Anything else
```c#
    for (int i = 0; i < psfo.Tables.Count; i++)
    {
        if (psfo.Tables[i].Name == "Content we are looking for ")
        {
           //get the value 
           string value = psfo.Tables[i].Value;
        }
    }
```

Editing a value 
```c#
    for (int i = 0; i < psfo.Tables.Count; i++)
    {
      if (psfo.Tables[i].Name == "TITLE_ID")
      {
          var tempitem = psfo.Tables[i];
          tempitem.Value = txtTitleId.Text.Trim();
          psfo.Tables[i] = tempitem;
      }
    }
```


