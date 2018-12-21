package io.interacionlab.anscomparison.wizardofoz;

import android.Manifest;
import android.app.Activity;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.os.Environment;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.CompoundButton;
import android.widget.EditText;
import android.widget.SeekBar;
import android.widget.Switch;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.net.InetAddress;

public class MainActivity extends AppCompatActivity{

    private static final String TAG = MainActivity.class.getSimpleName();

    private InetAddress ipVR, ipAR, ipCup, ipMill, ipPlant, ipSpeaker;
    private int port = 12000;
    private Integer pid = -1;

    private Button m_ButtonStart, m_ButtonCancel;
    private long m_StartTime = 0;

    private SeekBar m_SeekBarCup, m_SeekBarPepperMill, m_SeekBarSaltMill, m_SeekBarPlant, m_SeekBarSpeaker;
    private Switch m_SwitchCup, m_SwitchPepperMill, m_SwitchSaltMill, m_SwitchPlant, m_SwitchSpeaker;
    // Storage Permissions
    private static final int REQUEST_EXTERNAL_STORAGE = 1;
    private static String[] PERMISSIONS_STORAGE = {
            Manifest.permission.READ_EXTERNAL_STORAGE,
            Manifest.permission.WRITE_EXTERNAL_STORAGE
    };

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        m_ButtonStart = findViewById(R.id.buttonStart);
        m_ButtonCancel = findViewById(R.id.buttonCancel);

        m_SeekBarCup = findViewById(R.id.seekBarCup);
        m_SeekBarCup.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                onInput("cup", progress);
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
        });

        m_SeekBarPepperMill = findViewById(R.id.seekBarPepperMill);
        m_SeekBarPepperMill.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                onInput("peppermill", progress);
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
        });

        m_SeekBarSaltMill = findViewById(R.id.seekBarSaltMill);
        m_SeekBarSaltMill.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                onInput("saltmill", progress);
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
        });

        m_SeekBarPlant = findViewById(R.id.seekBarPlant);
        m_SeekBarPlant.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                onInput("plant", progress);
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
        });

        m_SeekBarSpeaker = findViewById(R.id.seekBarSpeaker);
        m_SeekBarSpeaker.setOnSeekBarChangeListener(new SeekBar.OnSeekBarChangeListener() {
            @Override
            public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser) {
                onInput("speaker", progress);
            }

            @Override
            public void onStartTrackingTouch(SeekBar seekBar) {}
            @Override
            public void onStopTrackingTouch(SeekBar seekBar) {}
        });

        m_SwitchCup = findViewById(R.id.switchCup);
        m_SwitchCup.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked)
                {
                    m_SeekBarCup.setEnabled(true);
                } else{
                    m_SeekBarCup.setEnabled(false);
                    onInput("cup", -1);
                }
            }
        });
        m_SwitchPepperMill = findViewById(R.id.switchPepperMill);
        m_SwitchPepperMill.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked)
                {
                    m_SeekBarPepperMill.setEnabled(true);
                } else{
                    m_SeekBarPepperMill.setEnabled(false);
                    onInput("peppermill", -1);
                }
            }
        });

        m_SwitchSaltMill = findViewById(R.id.switchSaltMill);
        m_SwitchSaltMill.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked)
                {
                    m_SeekBarSaltMill.setEnabled(true);
                } else{
                    m_SeekBarSaltMill.setEnabled(false);
                    onInput("saltmill", -1);
                }
            }
        });
        m_SwitchPlant = findViewById(R.id.switchPlant);
        m_SwitchPlant.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked)
                {
                    m_SeekBarPlant.setEnabled(true);
                } else{
                    m_SeekBarPlant.setEnabled(false);
                    onInput("plant", -1);
                }
            }
        });
        m_SwitchSpeaker = findViewById(R.id.switchSpeaker);
        m_SwitchSpeaker.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if (isChecked)
                {
                    m_SeekBarSpeaker.setEnabled(true);
                } else{
                    m_SeekBarSpeaker.setEnabled(false);
                    onInput("speaker", -1);
                }
            }
        });

        m_SeekBarCup.setEnabled(false);
        m_SeekBarPepperMill.setEnabled(false);
        m_SeekBarSaltMill.setEnabled(false);
        m_SeekBarPlant.setEnabled(false);
        m_SeekBarSpeaker.setEnabled(false);


        final EditText edipAR = findViewById(R.id.editTextIpAR);
        edipAR.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();

                try {
                    if (text.length() > 8) {
                        ipAR = InetAddress.getByName(text);
                        edipAR.setBackgroundColor(Color.GREEN);
                    }
                    else {
                        throw new Exception("To short");
                    }
                } catch (Exception e) {
                    ipAR = null;
                    edipAR.setBackgroundColor(Color.RED);
                }
            }
        });
        edipAR.setText(edipAR.getText());

        final EditText edipCup = findViewById(R.id.editTextIpCup);
        edipCup.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();

                try {
                    if (text.length() > 8) {
                        ipCup = InetAddress.getByName(text);
                        edipCup.setBackgroundColor(Color.GREEN);
                    }
                    else {
                        throw new Exception("To short");
                    }
                } catch (Exception e) {
                    ipCup = null;
                    edipCup.setBackgroundColor(Color.RED);
                }
            }
        });
        edipCup.setText(edipCup.getText());


        final EditText edipPepperMill = findViewById(R.id.editTextIpPepperMill);
        edipPepperMill.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();

                try {
                    if (text.length() > 8) {
                        ipMill = InetAddress.getByName(text);
                        edipPepperMill.setBackgroundColor(Color.GREEN);
                    }
                    else {
                        throw new Exception("To short");
                    }
                } catch (Exception e) {
                    ipMill = null;
                    edipPepperMill.setBackgroundColor(Color.RED);
                }
            }
        });
        edipPepperMill.setText(edipPepperMill.getText());


        final EditText edipPlant = findViewById(R.id.editTextIpPlant);
        edipPlant.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();

                try {
                    if (text.length() > 8) {
                        ipPlant = InetAddress.getByName(text);
                        edipPlant.setBackgroundColor(Color.GREEN);
                    }
                    else {
                        throw new Exception("To short");
                    }
                } catch (Exception e) {
                    ipPlant = null;
                    edipPlant.setBackgroundColor(Color.RED);
                }
            }
        });
        edipPlant.setText(edipPlant.getText());


        final EditText edipSpeaker = findViewById(R.id.editTextIpSpeaker);
        edipSpeaker.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();

                try {
                    if (text.length() > 8) {
                        ipSpeaker = InetAddress.getByName(text);
                        edipSpeaker.setBackgroundColor(Color.GREEN);
                    }
                    else {
                        throw new Exception("To short");
                    }
                } catch (Exception e) {
                    ipSpeaker = null;
                    edipSpeaker.setBackgroundColor(Color.RED);
                }
            }
        });
        edipSpeaker.setText(edipSpeaker.getText());


        final EditText edipVR = findViewById(R.id.editTextIpVR);
        edipVR.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();

                try {
                    if (text.length() > 8) {
                        ipVR = InetAddress.getByName(text);
                        edipVR.setBackgroundColor(Color.GREEN);
                    }
                    else {
                        throw new Exception("To short");
                    }
                } catch (Exception e) {
                    ipVR = null;
                    edipVR.setBackgroundColor(Color.RED);
                }
            }
        });
        edipVR.setText(edipVR.getText());


        final EditText epid = findViewById(R.id.editTextPId);
        epid.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {

                String text = s.toString();
                try {
                    pid = Integer.valueOf(text);
                    if (pid > 0) {
                        epid.setBackgroundColor(Color.GREEN);
                    } else
                        throw new Exception("Not a Valid PID");
                } catch ( Exception ignore) {
                    epid.setBackgroundColor(Color.RED);
                }
            }
        });
        epid.setText(epid.getText());


        /*final EditText edport = findViewById(R.id.editTextPort);
        edport.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {}

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {}

            @Override
            public void afterTextChanged(Editable s) {
                String text = s.toString();
                try {
                    port = Integer.valueOf(text);
                    if (port < 65535 & port > 123) {
                        edport.setBackgroundColor(Color.GREEN);
                    } else
                        throw new Exception("Not a Valid port number");
                } catch ( Exception ignore) {
                    port = 0;
                    edport.setBackgroundColor(Color.RED);
                }
            }
        });
        edport.setText(edport.getText());*/

    }

    public void onInput(String objectname, int value) {

        String data = "{\"name\":\"" + objectname + "\",\"value\":" + value + ",\"PId\":" + pid + "}";
        //Log.d(TAG, "message " + data);
        //Log.d(TAG, "port " + port);
        //Log.d(TAG, "ipAR " + ipAR);
        //Log.d(TAG, "ipVR " + ipVR);
        if (port != 0 && ipAR != null && ipVR != null) {

            (new UDPClient(this.ipAR, this.port)).execute(data);
            (new UDPClient(this.ipVR, this.port)).execute(data);
            InetAddress temp = null;
            switch (objectname) {
                case "cup": temp = this.ipCup;
                    break;
                case "peppermill": temp = this.ipMill;
                    break;
                case "saltmill": temp = this.ipMill;
                    break;
                case "plant": temp = this.ipPlant;
                    break;
                case "speaker": temp = this.ipSpeaker;
                    break;
            }
            (new UDPClient(temp, this.port)).execute(data);

        };
    }

    public void onClickStart(View view) {

        m_ButtonStart.setEnabled(false);
        m_ButtonCancel.setEnabled(true);
        m_StartTime = System.currentTimeMillis();

        long timeStart = System.currentTimeMillis();
        verifyStoragePermissions(this);

        File root  = new File(Environment.getExternalStorageDirectory().getAbsolutePath(), "ANSComparison/");
        //File f = this.getFilesDir();
        File file = new File(root, "times.csv");
        Log.d(TAG, file.toString());
        if (!file.getParentFile().exists()) {
            file.getParentFile().mkdirs();
        }
        try {
            FileOutputStream fileinput = new FileOutputStream(file, true);

            //for (int i = 0; i < 8; i++){
            //    String s = pid.toString()+","+timeStart+","+times[i]+"\n";
            //    fileinput.write(s.getBytes());
            //}
            fileinput.flush();
            fileinput.close();
        } catch (FileNotFoundException e) {
            Log.e(TAG, "FileNotFoundException");
            e.printStackTrace();
        } catch (IOException e) {
            Log.e(TAG, "File not available");
        }
    }

    public void onClickCancel(View view) {
        m_ButtonStart.setEnabled(true);
        m_ButtonCancel.setEnabled(false);
    }

    /**
     * Checks if the app has permission to write to device storage
     *
     * If the app does not has permission then the user will be prompted to grant permissions
     *
     * @param activity
     */
    public static void verifyStoragePermissions(Activity activity) {
        // Check if we have write permission
        int permission = ActivityCompat.checkSelfPermission(activity, Manifest.permission.WRITE_EXTERNAL_STORAGE);

        if (permission != PackageManager.PERMISSION_GRANTED) {
            // We don't have permission so prompt the user
            ActivityCompat.requestPermissions(
                    activity,
                    PERMISSIONS_STORAGE,
                    REQUEST_EXTERNAL_STORAGE
            );
        }
    }
}
