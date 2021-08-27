# Splatoon for FFXIV
Splatoon plugin allows you to put infinite amount of waymarks in the world of different size, colors, allows to add custom text to them as well. 

# Install
Dalamud repository:

`https://raw.githubusercontent.com/Eternita-S/MyDalamudPlugins/main/pluginmaster.json`

Detailed instructions available here: https://github.com/Eternita-S/MyDalamudPlugins

# WARNING!
This project is in beta test. 
* Expect bugs! But critical bugs that could potentially break/crash the game should be fixed by now.
* ~~Always keep backup of your configuration!~~ Plugin will do backups for you now!
* Gui sucks, I'll do something with it later (never)

# Web API (beta)
Splatoon now supports web API to remotely manage layouts and elements.
Request http://127.0.0.1:47774/ with parameters specified in table.
<table>
  <tr>
    <th>Parameter</td>
    <th>Usage</td>
  </tr>
  <tr>
    <td>enable</td>
    <td>Comma-separated names of already existing in Splatoon layouts or elements that you want to enable. If you want to enable layout simply pass it's name, if you want to enable specific element, use `layoutName~elementName` pattern.</td>
  </tr>
  <tr>
    <td>disable</td>
    <td>Same as `enable`, but will disable elements instead</td>
  </tr>
  <tr>
    <td colspan="2">Note: disabling always done before enabling. You can pass both parameters in one request. For example you can pass all known elements in disable parameter to clean up display, and then enable only ones that are currently needed. Passing same name of element in both enable and disable parameters will always result in element being enabled.</td>
  </tr>
  <tr>
    <td>elements</td>
    <td>Directly transfer encoded element into Splatoon without need of any configuration from inside plugin. They are temporary and will not be stored by Splatoon between restarts.
<ul>
      <li> Multiple comma-separated elements allowed unless also using `raw` parameter.</li>
      <li> Can contain layouts and elements at the same time. To obtain layout/element code, use appropriate button inside Splatoon configuration after setting them up.</li>
      <li> If you are exporting layout, it's display conditions, zone/job lock, etc preserved. You do not need to enable layouts/elements before exporting, it will be done automatically.</li>
      </ul>
  </td>
  </tr>
  <tr>
    <td>namespace</td>
    <td>Add element to specific named namespace instead of default one. If you are not using `destroyAt` parameter, always specify namespace so you can destroy element manually later.</td>
  </tr>
  <tr>
    <td>destroyAt</td>
    <td></td>
  </tr>
  <tr>
    <td>destroy</td>
    <td></td>
  </tr>
  <tr>
    <td>raw</td>
    <td></td>
  </tr>
</table>
